namespace WeShopAlot.UI.ClientWPF.Services
{
    /// <summary>Where the API lives and the Stripe key the card form uses (the same values as Angular's environment.ts).</summary>
    public record AppSettings(Uri ApiUrl, string StripePublishableKey)
    {
        // The WeShopAlot API's https launch profile.
        private const string DefaultApiUrl = "https://localhost:7244/api/";

        // Publishable keys are meant to be public (Angular ships this one to every browser).
        // Keep it in step with Client/src/environments/environment.ts.
        private const string DefaultStripePublishableKey =
            "pk_test_51UONLnIex1g4PmxhBKf2PIzypfN7QYqVCej3hfAZdZovSIWHWmT5CMBjX5PnGyFcPrXJUf30pWG2SCpIJoctWXdQ00WsS5rQXH";

        /// <summary>Defaults, overridable with the WESHOPALOT_API_URL and WESHOPALOT_STRIPE_PK environment variables.</summary>
        public static AppSettings Load()
        {
            var apiUrl = Environment.GetEnvironmentVariable("WESHOPALOT_API_URL") ?? DefaultApiUrl;
            if (!apiUrl.EndsWith('/')) apiUrl += "/";
            var stripeKey = Environment.GetEnvironmentVariable("WESHOPALOT_STRIPE_PK") ?? DefaultStripePublishableKey;
            return new AppSettings(new Uri(apiUrl), stripeKey);
        }
    }
}
