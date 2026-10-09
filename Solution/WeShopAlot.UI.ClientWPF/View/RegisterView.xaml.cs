using System.Windows;
using System.Windows.Controls;
using WeShopAlot.UI.ClientWPF.ViewModel;

namespace WeShopAlot.UI.ClientWPF.View
{
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel viewModel) viewModel.Password = PasswordInput.Password;
        }
    }
}
