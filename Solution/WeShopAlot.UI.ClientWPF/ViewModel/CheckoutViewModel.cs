using System.Collections.ObjectModel;
using System.Net.Http;
using WeShopAlot.UI.ClientWPF.Command;
using WeShopAlot.UI.ClientWPF.Models;
using WeShopAlot.UI.ClientWPF.Services;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>
    /// Checkout in four steps, like Angular's CheckoutComponent stepper:
    /// 0 Address (GET/PUT /api/account/address), 1 Delivery (GET /api/order/deliveryMethods),
    /// 2 Review (POST /api/payment/{basketId} creates the Stripe payment intent),
    /// 3 Payment (POST /api/order, then Stripe.js confirms the card payment).
    /// </summary>
    public class CheckoutViewModel : ValidatingViewModelBase
    {
        public override string? PageTitle => "Checkout";

        public const int AddressStep = 0;
        public const int DeliveryStep = 1;
        public const int ReviewStep = 2;
        public const int PaymentStep = 3;

        private readonly ApiClient api;
        private readonly AccountService account;
        private readonly BasketService basket;
        private readonly IShell shell;

        private int stepIndex;
        private bool isBusy;
        private string firstName = "", lastName = "", street = "", city = "", state = "", zipcode = "";
        private DeliveryMethod? selectedDeliveryMethod;
        private string nameOnCard = "";
        private bool cardComplete;
        private string? cardError;

        public CheckoutViewModel(ApiClient api, AccountService account, BasketService basket, IShell shell,
            string stripePublishableKey)
        {
            this.api = api;
            this.account = account;
            this.basket = basket;
            this.shell = shell;
            StripePublishableKey = stripePublishableKey;

            SaveAddressCommand = new DelegateCommand(async _ => await SaveAddressAsync(), _ => !IsBusy && IsAddressValid);
            ToDeliveryCommand = new DelegateCommand(_ => StepIndex = DeliveryStep, _ => IsAddressValid);
            ToReviewCommand = new DelegateCommand(_ => StepIndex = ReviewStep, _ => SelectedDeliveryMethod is not null);
            ToPaymentCommand = new DelegateCommand(async _ => await ToPaymentAsync(), _ => !IsBusy);
            BackCommand = new DelegateCommand(_ => StepIndex--, _ => StepIndex > AddressStep && !IsBusy);
            BackToBasketCommand = new DelegateCommand(_ => shell.GoBasket());
            SubmitOrderCommand = new DelegateCommand(async _ => await SubmitOrderAsync(), _ => CanSubmit);

            basket.Changed += (_, _) => RaisePropertyChanged(nameof(Totals));
        }

        public string StripePublishableKey { get; }

        /// <summary>Set by StripeCardControl when it loads.</summary>
        public IStripeCardHost? CardHost { get; set; }

        public DelegateCommand SaveAddressCommand { get; }
        public DelegateCommand ToDeliveryCommand { get; }
        public DelegateCommand ToReviewCommand { get; }
        public DelegateCommand ToPaymentCommand { get; }
        public DelegateCommand BackCommand { get; }
        public DelegateCommand BackToBasketCommand { get; }
        public DelegateCommand SubmitOrderCommand { get; }

        public int StepIndex
        {
            get => stepIndex;
            set
            {
                if (SetProperty(ref stepIndex, Math.Clamp(value, AddressStep, PaymentStep))) RaiseCommandStates();
            }
        }

        public bool IsBusy
        {
            get => isBusy;
            private set
            {
                if (SetProperty(ref isBusy, value)) RaiseCommandStates();
            }
        }

        // ------------------------------------------------------------ step 0: address

        public string FirstName { get => firstName; set => SetAddressField(ref firstName, value, FormRules.MaxLength(value, "First name", 50)); }
        public string LastName { get => lastName; set => SetAddressField(ref lastName, value, FormRules.MaxLength(value, "Last name", 50)); }
        public string Street { get => street; set => SetAddressField(ref street, value, FormRules.MaxLength(value, "Street", 100)); }
        public string City { get => city; set => SetAddressField(ref city, value, FormRules.MaxLength(value, "City", 50)); }
        public string State { get => state; set => SetAddressField(ref state, value, FormRules.State(value)); }
        public string Zipcode { get => zipcode; set => SetAddressField(ref zipcode, value, FormRules.Zipcode(value)); }

        public bool IsAddressValid =>
            FormRules.MaxLength(FirstName, "", 50) is null && FormRules.MaxLength(LastName, "", 50) is null
            && FormRules.MaxLength(Street, "", 100) is null && FormRules.MaxLength(City, "", 50) is null
            && FormRules.State(State) is null && FormRules.Zipcode(Zipcode) is null;

        // ------------------------------------------------------------ step 1: delivery

        public ObservableCollection<DeliveryMethod> DeliveryMethods { get; } = new();

        public DeliveryMethod? SelectedDeliveryMethod
        {
            get => selectedDeliveryMethod;
            set
            {
                if (!SetProperty(ref selectedDeliveryMethod, value)) return;
                RaiseCommandStates();
                if (value is not null) _ = SetDeliveryMethodAsync(value);
            }
        }

        // ------------------------------------------------------------ step 2: review

        public IReadOnlyList<BasketItem> Items => basket.Basket?.Items ?? new List<BasketItem>();

        public BasketTotals? Totals => basket.Totals;

        // ------------------------------------------------------------ step 3: payment

        public string NameOnCard
        {
            get => nameOnCard;
            set
            {
                if (!SetProperty(ref nameOnCard, value ?? "")) return;
                SetError(nameof(NameOnCard), FormRules.Required(nameOnCard, "Name on card"));
                SubmitOrderCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>Set by StripeCardControl as the card number, expiry and CVC become complete.</summary>
        public bool CardComplete
        {
            get => cardComplete;
            set
            {
                if (SetProperty(ref cardComplete, value)) SubmitOrderCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>Stripe's message for the card as typed (e.g. "Your card number is incomplete.").</summary>
        public string? CardError
        {
            get => cardError;
            set => SetProperty(ref cardError, value);
        }

        public bool CanSubmit =>
            !IsBusy && CardComplete && !string.IsNullOrWhiteSpace(NameOnCard) && SelectedDeliveryMethod is not null;

        // ------------------------------------------------------------ loading and actions

        public override async Task LoadAsync()
        {
            IsBusy = true;
            try
            {
                var address = await account.GetAddressAsync();
                if (address is not null) ApplyAddress(address);

                var methods = await api.GetDeliveryMethodsAsync();
                DeliveryMethods.Clear();
                foreach (var method in methods.OrderByDescending(m => m.Price)) DeliveryMethods.Add(method);

                // Remember a delivery choice already saved on the basket, without saving it again.
                var savedId = basket.Basket?.DeliveryMethodId;
                selectedDeliveryMethod = DeliveryMethods.FirstOrDefault(m => m.Id == savedId);
                RaisePropertyChanged(nameof(SelectedDeliveryMethod));
                RaisePropertyChanged(nameof(Items));
                RaisePropertyChanged(nameof(Totals));
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveAddressAsync()
        {
            await RunAsync(async () =>
            {
                await account.UpdateAddressAsync(BuildAddress());
                shell.ShowMessage("Address saved as your default.");
            });
        }

        private async Task SetDeliveryMethodAsync(DeliveryMethod method)
        {
            await RunAsync(() => basket.SetDeliveryMethodAsync(method));
        }

        /// <summary>Review → Payment: the API creates (or updates) the Stripe payment intent for this basket.</summary>
        private async Task ToPaymentAsync()
        {
            await RunAsync(async () =>
            {
                await basket.CreatePaymentIntentAsync();
                StepIndex = PaymentStep;
            });
        }

        /// <summary>The same order of events as Angular's submitOrder(): create the order, then take the payment.</summary>
        private async Task SubmitOrderAsync()
        {
            if (CardHost is null || SelectedDeliveryMethod is null || basket.Basket is null) return;
            await RunAsync(async () =>
            {
                var current = basket.Basket!;
                if (string.IsNullOrEmpty(current.ClientSecret)) await basket.CreatePaymentIntentAsync();

                var order = await api.CreateOrderAsync(new OrderToCreate(current.Id, SelectedDeliveryMethod!.Id, BuildAddress()))
                    ?? throw new InvalidOperationException("The API did not return the new order.");

                var result = await CardHost.ConfirmCardPaymentAsync(basket.Basket!.ClientSecret!, NameOnCard.Trim());
                if (!result.Succeeded)
                {
                    shell.ShowMessage(result.Error ?? "The payment did not go through.", isError: true);
                    return;
                }

                await basket.DeleteAsync();
                shell.GoCheckoutSuccess(order.Id);
            });
        }

        private async Task RunAsync(Func<Task> action)
        {
            IsBusy = true;
            try
            {
                await action();
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or InvalidOperationException)
            {
                shell.ShowMessage(ex is ApiException apiError ? apiError.Details : ex.Message, isError: true);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Address BuildAddress() => new()
        {
            FirstName = FirstName.Trim(),
            LastName = LastName.Trim(),
            Street = Street.Trim(),
            City = City.Trim(),
            State = State.Trim().ToUpperInvariant(),
            Zipcode = Zipcode.Trim()
        };

        private void ApplyAddress(Address address)
        {
            FirstName = address.FirstName ?? "";
            LastName = address.LastName ?? "";
            Street = address.Street ?? "";
            City = address.City ?? "";
            State = address.State ?? "";
            Zipcode = address.Zipcode ?? "";
        }

        private void SetAddressField(ref string field, string? value, string? error,
            [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            if (!SetProperty(ref field, value ?? "", propertyName)) return;
            SetError(propertyName!, error);
            RaisePropertyChanged(nameof(IsAddressValid));
            RaiseCommandStates();
        }

        private void RaiseCommandStates()
        {
            SaveAddressCommand?.RaiseCanExecuteChanged();
            ToDeliveryCommand?.RaiseCanExecuteChanged();
            ToReviewCommand?.RaiseCanExecuteChanged();
            ToPaymentCommand?.RaiseCanExecuteChanged();
            BackCommand?.RaiseCanExecuteChanged();
            SubmitOrderCommand?.RaiseCanExecuteChanged();
        }
    }
}
