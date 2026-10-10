using System.Net.Http;
using System.Windows;
using WeShopAlot.UI.ClientWPF.Services;
using WeShopAlot.UI.ClientWPF.ViewModel;

namespace WeShopAlot.UI.ClientWPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Settings: https://localhost:7244/api/ and the Stripe publishable key (see Services/AppSettings.cs).
            var settings = AppSettings.Load();

            // One HttpClient for the life of the app (creating one per request exhausts sockets).
            var http = new HttpClient
            {
                BaseAddress = settings.ApiUrl,
                Timeout = TimeSpan.FromSeconds(30)
            };

            // Wire the app together by hand: small enough that a DI container would only hide it.
            var api = new ApiClient(http);
            var session = new SessionStore();
            var account = new AccountService(api, session);
            var basket = new BasketService(api, session);
            var view = new ViewSettings(session);
            var mainViewModel = new MainViewModel(api, account, basket, view, settings.StripePublishableKey);

            new MainWindow(mainViewModel).Show();
        }
    }
}
