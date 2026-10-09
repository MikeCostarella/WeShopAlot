using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;
using WeShopAlot.UI.ClientWPF.Services;
using WeShopAlot.UI.ClientWPF.ViewModel;

namespace WeShopAlot.UI.ClientWPF.View
{
    /// <summary>
    /// Hosts Stripe's card form in WebView2 and lets CheckoutViewModel confirm the payment through it.
    /// Payment/stripe-card.html is served from https://weshopalot.payment/ (a folder mapped to a virtual host),
    /// because Stripe.js expects a secure page.
    /// </summary>
    public partial class StripeCardControl : UserControl, IStripeCardHost
    {
        private const string VirtualHost = "weshopalot.payment";
        private const double CardHeight = 64;
        private const double ChallengeHeight = 520; // room for a 3-D Secure check (test card 4000 0025 0000 3155)

        private CheckoutViewModel? viewModel;
        private bool initializing;
        private TaskCompletionSource<CardPaymentResult>? pending;

        public StripeCardControl()
        {
            InitializeComponent();
            DataContextChanged += (_, _) => AttachViewModel();
            IsVisibleChanged += async (_, e) =>
            {
                if (e.NewValue is true) await InitializeAsync();
            };
        }

        public Task<CardPaymentResult> ConfirmCardPaymentAsync(string clientSecret, string nameOnCard)
        {
            if (CardBrowser.CoreWebView2 is null)
                return Task.FromResult(new CardPaymentResult(false, "The card form has not finished loading."));

            pending?.TrySetResult(new CardPaymentResult(false, "Payment was restarted."));
            pending = new TaskCompletionSource<CardPaymentResult>();
            CardBrowser.Height = ChallengeHeight;
            CardBrowser.CoreWebView2.PostWebMessageAsJson(
                JsonSerializer.Serialize(new { type = "pay", clientSecret, name = nameOnCard }));
            return pending.Task;
        }

        private void AttachViewModel()
        {
            viewModel = DataContext as CheckoutViewModel;
            if (viewModel is not null) viewModel.CardHost = this;
        }

        /// <summary>Starts the browser the first time the Payment step is shown.</summary>
        private async Task InitializeAsync()
        {
            if (initializing || CardBrowser.CoreWebView2 is not null || viewModel is null) return;
            initializing = true;
            try
            {
                await CardBrowser.EnsureCoreWebView2Async();
                var folder = Path.Combine(AppContext.BaseDirectory, "Payment");
                CardBrowser.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    VirtualHost, folder, CoreWebView2HostResourceAccessKind.Allow);
                CardBrowser.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
                CardBrowser.Source = new Uri(
                    $"https://{VirtualHost}/stripe-card.html?pk={Uri.EscapeDataString(viewModel.StripePublishableKey)}");
            }
            catch (Exception ex)
            {
                // Most often: the Microsoft Edge WebView2 Runtime is missing (it ships with Windows 10/11).
                StatusText.Text = "The card form could not start: " + ex.Message;
            }
            finally
            {
                initializing = false;
            }
        }

        private void OnWebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            using var document = JsonDocument.Parse(e.WebMessageAsJson);
            var message = document.RootElement;
            var error = message.TryGetProperty("error", out var errorValue) && errorValue.ValueKind == JsonValueKind.String
                ? errorValue.GetString()
                : null;

            switch (message.GetProperty("type").GetString())
            {
                case "ready":
                    StatusText.Text = "Card details (test card: 4242 4242 4242 4242, any future date, any CVC)";
                    break;
                case "change":
                    if (viewModel is not null)
                    {
                        viewModel.CardComplete = message.GetProperty("complete").GetBoolean();
                        viewModel.CardError = error;
                    }
                    break;
                case "error":
                    StatusText.Text = "Stripe could not start: " + error;
                    break;
                case "result":
                    CardBrowser.Height = CardHeight;
                    pending?.TrySetResult(new CardPaymentResult(message.GetProperty("ok").GetBoolean(), error));
                    pending = null;
                    break;
            }
        }
    }
}
