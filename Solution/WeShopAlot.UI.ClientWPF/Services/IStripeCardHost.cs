namespace WeShopAlot.UI.ClientWPF.Services
{
    public record CardPaymentResult(bool Succeeded, string? Error);

    /// <summary>
    /// The Stripe card form (View/StripeCardControl, a WebView2 running Stripe.js). Card numbers never touch
    /// this app or the WeShopAlot API: Stripe.js sends them straight to Stripe, as in the Angular client.
    /// </summary>
    public interface IStripeCardHost
    {
        Task<CardPaymentResult> ConfirmCardPaymentAsync(string clientSecret, string nameOnCard);
    }
}
