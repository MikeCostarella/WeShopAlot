using WeShopAlot.UI.ClientWPF.Command;
using WeShopAlot.UI.ClientWPF.Services;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>"Thank you" page after payment (Angular's CheckoutSuccessComponent).</summary>
    public class CheckoutSuccessViewModel : ViewModelBase
    {
        public override string? PageTitle => "Success";

        public override string Breadcrumb => "Home  /  Checkout  /  Success";

        public CheckoutSuccessViewModel(int orderId, IShell shell)
        {
            OrderId = orderId;
            ViewOrderCommand = new DelegateCommand(_ => shell.GoOrder(orderId));
            ShopCommand = new DelegateCommand(_ => shell.GoShop());
        }

        public int OrderId { get; }

        public string Heading => $"Thank you! Order #{OrderId} is confirmed.";

        public DelegateCommand ViewOrderCommand { get; }
        public DelegateCommand ShopCommand { get; }
    }
}
