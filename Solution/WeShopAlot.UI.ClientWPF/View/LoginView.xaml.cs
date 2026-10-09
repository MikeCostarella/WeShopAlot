using System.Windows;
using System.Windows.Controls;
using WeShopAlot.UI.ClientWPF.ViewModel;

namespace WeShopAlot.UI.ClientWPF.View
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel) viewModel.Password = PasswordInput.Password;
        }
    }
}
