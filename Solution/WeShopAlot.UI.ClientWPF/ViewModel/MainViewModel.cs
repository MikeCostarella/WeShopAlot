using System.Net.Http;
using System.Windows.Threading;
using WeShopAlot.UI.ClientWPF.Command;
using WeShopAlot.UI.ClientWPF.Services;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>
    /// Backs MainWindow: the nav bar, the notice banner, and whichever page is showing (CurrentPage).
    /// DataTemplates in MainWindow.xaml pick the view for each page view model, so navigating is just
    /// setting CurrentPage, the desktop version of Angular's router outlet.
    /// </summary>
    public class MainViewModel : ViewModelBase, IShell
    {
        private const int StartupAttempts = 15;

        private readonly ApiClient api;
        private readonly AccountService account;
        private readonly BasketService basket;
        private readonly string stripePublishableKey;
        private readonly DispatcherTimer messageTimer = new() { Interval = TimeSpan.FromSeconds(5) };

        private ViewModelBase? currentPage;
        private string? message;
        private bool messageIsError;
        private string connectionStatus = "";

        public MainViewModel(ApiClient api, AccountService account, BasketService basket, string stripePublishableKey)
        {
            this.api = api;
            this.account = account;
            this.basket = basket;
            this.stripePublishableKey = stripePublishableKey;

            HomeCommand = new DelegateCommand(_ => GoHome());
            ShopCommand = new DelegateCommand(_ => GoShop());
            BasketCommand = new DelegateCommand(_ => GoBasket());
            OrdersCommand = new DelegateCommand(_ => GoOrders());
            LoginCommand = new DelegateCommand(_ => GoLogin());
            RegisterCommand = new DelegateCommand(_ => GoRegister());
            LogoutCommand = new DelegateCommand(_ => Logout());
            DismissMessageCommand = new DelegateCommand(_ => Message = null);

            messageTimer.Tick += (_, _) => { messageTimer.Stop(); Message = null; };
            account.Changed += (_, _) =>
            {
                RaisePropertyChanged(nameof(IsSignedIn));
                RaisePropertyChanged(nameof(WelcomeText));
            };
            basket.Changed += (_, _) => RaisePropertyChanged(nameof(BasketCount));
        }

        public DelegateCommand HomeCommand { get; }
        public DelegateCommand ShopCommand { get; }
        public DelegateCommand BasketCommand { get; }
        public DelegateCommand OrdersCommand { get; }
        public DelegateCommand LoginCommand { get; }
        public DelegateCommand RegisterCommand { get; }
        public DelegateCommand LogoutCommand { get; }
        public DelegateCommand DismissMessageCommand { get; }

        public ViewModelBase? CurrentPage
        {
            get => currentPage;
            private set => SetProperty(ref currentPage, value);
        }

        public bool IsSignedIn => account.IsSignedIn;

        public string WelcomeText => account.CurrentUser is { } user ? $"Welcome {user.DisplayName}" : "";

        public int BasketCount => basket.ItemCount;

        /// <summary>The notice banner (Angular uses ngx-toastr for these).</summary>
        public string? Message
        {
            get => message;
            private set => SetProperty(ref message, value);
        }

        public bool MessageIsError
        {
            get => messageIsError;
            private set => SetProperty(ref messageIsError, value);
        }

        /// <summary>Shown in the status bar while the app waits for the API at startup.</summary>
        public string ConnectionStatus
        {
            get => connectionStatus;
            private set => SetProperty(ref connectionStatus, value);
        }

        public string ApiAddress => api.BaseAddress.ToString();

        /// <summary>
        /// Opens on Home, then restores the saved sign-in and basket. When Visual Studio starts the API and this
        /// app together, the API may still be starting, so keep trying for about 30 seconds.
        /// </summary>
        public override async Task LoadAsync()
        {
            GoHome();
            for (var attempt = 1; attempt <= StartupAttempts; attempt++)
            {
                try
                {
                    await account.LoadCurrentUserAsync();
                    await basket.LoadAsync();
                    ConnectionStatus = "";
                    return;
                }
                catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
                {
                    ConnectionStatus = $"Waiting for the API (attempt {attempt} of {StartupAttempts})…";
                    if (attempt < StartupAttempts) await Task.Delay(TimeSpan.FromSeconds(2));
                }
            }
            ConnectionStatus = "Can't reach the API. Start WeShopAlot.WebAPI (https profile).";
        }

        // ---------------------------------------------------------------- IShell

        public void GoHome() => Navigate(new HomeViewModel(this));

        public void GoShop() => Navigate(new ProductsViewModel(api, basket, this));

        public void GoProduct(int productId) => Navigate(new ProductDetailsViewModel(productId, api, basket, this));

        public void GoBasket() => Navigate(new BasketViewModel(basket, this));

        public void GoCheckout()
        {
            if (!account.IsSignedIn) { GoLogin(GoCheckout); return; }
            if (basket.ItemCount == 0) { ShowMessage("Your basket is empty."); GoShop(); return; }
            Navigate(new CheckoutViewModel(api, account, basket, this, stripePublishableKey));
        }

        public void GoCheckoutSuccess(int orderId) => Navigate(new CheckoutSuccessViewModel(orderId, this));

        public void GoLogin(Action? afterSignIn = null) =>
            Navigate(new LoginViewModel(account, this, afterSignIn ?? GoShop));

        public void GoRegister(Action? afterSignIn = null) =>
            Navigate(new RegisterViewModel(account, this, afterSignIn ?? GoShop));

        public void GoOrders()
        {
            if (!account.IsSignedIn) { GoLogin(GoOrders); return; }
            Navigate(new OrdersViewModel(api, this));
        }

        public void GoOrder(int orderId)
        {
            if (!account.IsSignedIn) { GoLogin(() => GoOrder(orderId)); return; }
            Navigate(new OrderDetailViewModel(orderId, api, this));
        }

        public void ShowMessage(string text, bool isError = false)
        {
            MessageIsError = isError;
            Message = text;
            messageTimer.Stop();
            messageTimer.Start();
        }

        private void Logout()
        {
            account.SignOut();
            ShowMessage("You have signed out.");
            GoHome();
        }

        private async void Navigate(ViewModelBase page)
        {
            CurrentPage = page;
            try
            {
                await page.LoadAsync();
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or InvalidOperationException)
            {
                ShowMessage(ex is ApiException apiError ? apiError.Details : ex.Message, isError: true);
            }
        }
    }
}
