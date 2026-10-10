using WeShopAlot.UI.ClientWPF.Command;
using WeShopAlot.UI.ClientWPF.Models;
using WeShopAlot.UI.ClientWPF.Services;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>One product (Angular's ProductDetailsComponent): GET /api/product/{id}, pick a quantity, add or update.</summary>
    public class ProductDetailsViewModel : ViewModelBase
    {
        public override string? PageTitle => Product?.Name ?? "Product";

        public override string Breadcrumb => $"Home  /  Shop  /  {PageTitle}";

        private readonly int productId;
        private readonly ApiClient api;
        private readonly BasketService basket;
        private readonly IShell shell;

        private Product? product;
        private int quantity = 1;
        private int quantityInBasket;
        private bool isBusy;

        public ProductDetailsViewModel(int productId, ApiClient api, BasketService basket, IShell shell)
        {
            this.productId = productId;
            this.api = api;
            this.basket = basket;
            this.shell = shell;

            IncrementCommand = new DelegateCommand(_ => Quantity++);
            DecrementCommand = new DelegateCommand(_ => Quantity--, _ => Quantity > (quantityInBasket > 0 ? 0 : 1));
            UpdateBasketCommand = new DelegateCommand(async _ => await UpdateBasketAsync(), _ => Product is not null && !isBusy);
            BackCommand = new DelegateCommand(_ => shell.GoShop());
        }

        public DelegateCommand IncrementCommand { get; }
        public DelegateCommand DecrementCommand { get; }
        public DelegateCommand UpdateBasketCommand { get; }
        public DelegateCommand BackCommand { get; }

        public Product? Product
        {
            get => product;
            private set
            {
                if (!SetProperty(ref product, value)) return;
                UpdateBasketCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(PageTitle));
            }
        }

        public int Quantity
        {
            get => quantity;
            set
            {
                if (SetProperty(ref quantity, Math.Max(0, value))) DecrementCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>"Add to basket" the first time, "Update basket" once it is in the basket.</summary>
        public string ButtonText => quantityInBasket == 0 ? "Add to basket" : "Update basket";

        public override async Task LoadAsync()
        {
            Product = await api.GetProductAsync(productId);
            if (Product is null)
            {
                shell.ShowMessage("That product was not found.", isError: true);
                shell.GoShop();
                return;
            }
            quantityInBasket = basket.QuantityOf(productId);
            if (quantityInBasket > 0) Quantity = quantityInBasket;
            RaisePropertyChanged(nameof(ButtonText));
            DecrementCommand.RaiseCanExecuteChanged();
        }

        private async Task UpdateBasketAsync()
        {
            if (Product is null) return;
            isBusy = true;
            UpdateBasketCommand.RaiseCanExecuteChanged();
            try
            {
                await basket.SetQuantityAsync(Product, Quantity);
                quantityInBasket = basket.QuantityOf(productId);
                RaisePropertyChanged(nameof(ButtonText));
                DecrementCommand.RaiseCanExecuteChanged();
                shell.ShowMessage(quantityInBasket == 0
                    ? $"Removed {Product.Name} from your basket."
                    : $"Your basket has {quantityInBasket} × {Product.Name}.");
            }
            catch (Exception ex) when (ex is System.Net.Http.HttpRequestException or TaskCanceledException)
            {
                shell.ShowMessage(ex is ApiException apiError ? apiError.Details : ex.Message, isError: true);
            }
            finally
            {
                isBusy = false;
                UpdateBasketCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
