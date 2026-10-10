using System.Windows.Controls;
using System.Windows.Input;

namespace WeShopAlot.UI.ClientWPF.View
{
    public partial class ProductsView : UserControl
    {
        public ProductsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The filter lists and the List view's grid have scroll viewers of their own that swallow the mouse
        /// wheel even when they have nothing to scroll; scroll the whole page instead, like a web page.
        /// </summary>
        private void PageScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            PageScroll.ScrollToVerticalOffset(PageScroll.VerticalOffset - e.Delta / 3.0);
            e.Handled = true;
        }
    }
}
