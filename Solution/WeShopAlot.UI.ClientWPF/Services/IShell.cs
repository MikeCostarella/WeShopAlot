namespace WeShopAlot.UI.ClientWPF.Services
{
    /// <summary>
    /// Navigation and notices for the view models: the desktop version of Angular's router and ngx-toastr.
    /// MainViewModel implements it.
    /// </summary>
    public interface IShell
    {
        void GoHome();
        void GoShop();
        void GoProduct(int productId);
        void GoBasket();
        /// <summary>Requires sign-in, like Angular's AuthGuard on /checkout.</summary>
        void GoCheckout();
        void GoCheckoutSuccess(int orderId);
        void GoLogin(Action? afterSignIn = null);
        void GoRegister(Action? afterSignIn = null);
        /// <summary>Requires sign-in, like Angular's AuthGuard on /orders.</summary>
        void GoOrders();
        void GoOrder(int orderId);
        void ShowMessage(string message, bool isError = false);
    }
}
