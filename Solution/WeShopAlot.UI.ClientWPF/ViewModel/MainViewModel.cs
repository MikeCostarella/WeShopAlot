using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Windows.Threading;
using WeShopAlot.UI.ClientWPF.Command;
using WeShopAlot.UI.ClientWPF.Models;
using WeShopAlot.UI.ClientWPF.Services;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>
    /// Backs MainWindow: the header, the hamburger menu, the notice banner, the page band and whichever page is
    /// showing (CurrentPage). DataTemplates in MainWindow.xaml pick the view for each page view model, so
    /// navigating is just setting CurrentPage, the desktop version of Angular's router outlet.
    /// </summary>
    public class MainViewModel : ViewModelBase, IShell
    {
        private const int StartupAttempts = 15;
        private const string CourseSiteUrl = "https://mikecostarella.github.io/CS_AngularAndDotNetAPI/";
        private const string GitHubUrl = "https://github.com/MikeCostarella/WeShopAlot";

        private readonly ApiClient api;
        private readonly AccountService account;
        private readonly BasketService basket;
        private readonly string stripePublishableKey;
        private readonly DispatcherTimer messageTimer = new() { Interval = TimeSpan.FromSeconds(5) };

        private ViewModelBase? currentPage;
        private string? message;
        private bool messageIsError;
        private string connectionStatus = "";
        private bool isMenuOpen;

        public MainViewModel(ApiClient api, AccountService account, BasketService basket, ViewSettings view,
            string stripePublishableKey)
        {
            this.api = api;
            this.account = account;
            this.basket = basket;
            View = view;
            this.stripePublishableKey = stripePublishableKey;

            HomeCommand = new DelegateCommand(_ => GoHome());
            ShopCommand = new DelegateCommand(_ => GoShop());
            BasketCommand = new DelegateCommand(_ => GoBasket());
            OrdersCommand = new DelegateCommand(_ => GoOrders());
            LoginCommand = new DelegateCommand(_ => GoLogin());
            RegisterCommand = new DelegateCommand(_ => GoRegister());
            LogoutCommand = new DelegateCommand(_ => Logout());
            DismissMessageCommand = new DelegateCommand(_ => Message = null);
            OpenMenuCommand = new DelegateCommand(_ => IsMenuOpen = true);
            CloseMenuCommand = new DelegateCommand(_ => IsMenuOpen = false);
            ShowTypeCommand = new DelegateCommand(p => ShowType(p is NamedItem type ? type.Id : 0));
            OpenLinkCommand = new DelegateCommand(p => OpenLink(p as string));

            messageTimer.Tick += (_, _) => { messageTimer.Stop(); Message = null; };
            account.Changed += (_, _) =>
            {
                RaisePropertyChanged(nameof(IsSignedIn));
                RaisePropertyChanged(nameof(WelcomeText));
                RaisePropertyChanged(nameof(SignedInAs));
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
        public DelegateCommand OpenMenuCommand { get; }
        public DelegateCommand CloseMenuCommand { get; }
        public DelegateCommand ShowTypeCommand { get; }
        public DelegateCommand OpenLinkCommand { get; }

        /// <summary>Cards or List, shared with the shop page.</summary>
        public ViewSettings View { get; }

        public ViewModelBase? CurrentPage
        {
            get => currentPage;
            private set
            {
                var old = currentPage;
                if (!SetProperty(ref currentPage, value)) return;
                if (old is not null) old.PropertyChanged -= OnPagePropertyChanged;
                if (value is not null) value.PropertyChanged += OnPagePropertyChanged;
                RaisePageBand();
            }
        }

        // ---------------------------------------------------------------- header and page band

        public bool IsSignedIn => account.IsSignedIn;

        public string WelcomeText => account.CurrentUser is { } user ? $"Welcome {user.DisplayName}" : "";

        public string SignedInAs => account.CurrentUser is { } user ? $"Signed in as {user.DisplayName}" : "";

        public int BasketCount => basket.ItemCount;

        /// <summary>The page band's title (Shop, Basket, Checkout…); null on Home, which has no band.</summary>
        public override string? PageTitle => CurrentPage?.PageTitle;

        public override string Breadcrumb => CurrentPage?.Breadcrumb ?? "";

        public bool IsHomePage => CurrentPage is HomeViewModel;

        public bool IsShopPage => CurrentPage is ProductsViewModel or ProductDetailsViewModel;

        // ---------------------------------------------------------------- hamburger menu

        public bool IsMenuOpen
        {
            get => isMenuOpen;
            set => SetProperty(ref isMenuOpen, value);
        }

        /// <summary>Product types for the menu's Shop section (one link per type, as in the Angular menu).</summary>
        public ObservableCollection<NamedItem> ProductTypes { get; } = new();

        public string CourseSite => CourseSiteUrl;

        public string GitHub => GitHubUrl;

        public string SwaggerUrl => new Uri(api.BaseAddress, "../swagger").ToString();

        // ---------------------------------------------------------------- notices and status bar

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

        public string ApiOrigin => api.BaseAddress.GetLeftPart(UriPartial.Authority);

        public string BuildText => BuildInfo.Text;

        /// <summary>
        /// Opens on Home, then restores the saved sign-in and basket and loads the menu's product types. When
        /// Visual Studio starts the API and this app together, the API may still be starting, so keep trying
        /// for about 30 seconds.
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
                    var types = await api.GetTypesAsync();
                    ProductTypes.Clear();
                    foreach (var type in types) ProductTypes.Add(type);
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

        public void GoShop() => Navigate(new ProductsViewModel(api, basket, this, View));

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

        // ---------------------------------------------------------------- helpers

        /// <summary>The menu's Shop links: open the shop filtered to one type (0 = all products).</summary>
        private void ShowType(int typeId) =>
            Navigate(new ProductsViewModel(api, basket, this, View, typeId == 0 ? null : typeId));

        private void OpenLink(string? url)
        {
            if (string.IsNullOrEmpty(url)) return;
            IsMenuOpen = false;
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex) when (ex is Win32Exception or InvalidOperationException)
            {
                ShowMessage("Could not open the browser: " + ex.Message, isError: true);
            }
        }

        private void Logout()
        {
            account.SignOut();
            ShowMessage("You have signed out.");
            GoHome();
        }

        private void OnPagePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(ViewModelBase.PageTitle) or nameof(ViewModelBase.Breadcrumb)) RaisePageBand();
        }

        private void RaisePageBand()
        {
            RaisePropertyChanged(nameof(PageTitle));
            RaisePropertyChanged(nameof(Breadcrumb));
            RaisePropertyChanged(nameof(IsHomePage));
            RaisePropertyChanged(nameof(IsShopPage));
        }

        private async void Navigate(ViewModelBase page)
        {
            IsMenuOpen = false;
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
