using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WeShopAlot.UI.ClientWPF.ViewModel;

namespace WeShopAlot.UI.ClientWPF
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = viewModel;
            Loaded += MainWindow_Loaded;
            viewModel.PropertyChanged += (_, e) =>
            {
                // Like the Angular drawer: focus moves to the close button when the menu opens.
                if (e.PropertyName == nameof(MainViewModel.IsMenuOpen) && viewModel.IsMenuOpen)
                    Dispatcher.InvokeAsync(() => CloseMenuButton.Focus());
            };
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.LoadAsync();
        }

        /// <summary>Escape closes the hamburger menu.</summary>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape && viewModel.IsMenuOpen)
            {
                viewModel.IsMenuOpen = false;
                e.Handled = true;
                return;
            }
            base.OnPreviewKeyDown(e);
        }

        /// <summary>"Welcome Mike ▾" drops down View basket / View orders / Logout.</summary>
        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            var menu = UserButton.ContextMenu;
            menu.DataContext = viewModel;
            menu.PlacementTarget = UserButton;
            menu.Placement = PlacementMode.Bottom;
            menu.IsOpen = true;
        }

        /// <summary>Clicking outside the drawer closes it.</summary>
        private void Backdrop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewModel.IsMenuOpen = false;
        }
    }
}
