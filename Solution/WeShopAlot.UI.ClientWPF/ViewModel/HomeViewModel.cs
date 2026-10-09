using WeShopAlot.UI.ClientWPF.Command;
using WeShopAlot.UI.ClientWPF.Services;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>The landing page (Angular's HomeComponent).</summary>
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(IShell shell)
        {
            ShopCommand = new DelegateCommand(_ => shell.GoShop());
        }

        public DelegateCommand ShopCommand { get; }
    }
}
