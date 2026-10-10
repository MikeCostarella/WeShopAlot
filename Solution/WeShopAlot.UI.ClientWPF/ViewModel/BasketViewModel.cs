using System.Collections.ObjectModel;
using System.Net.Http;
using WeShopAlot.UI.ClientWPF.Command;
using WeShopAlot.UI.ClientWPF.Models;
using WeShopAlot.UI.ClientWPF.Services;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>The basket page (Angular's BasketComponent): change quantities, remove lines, see totals, check out.</summary>
    public class BasketViewModel : ViewModelBase
    {
        public override string? PageTitle => "Basket";

        private readonly BasketService basket;
        private readonly IShell shell;
        private bool isBusy;

        public BasketViewModel(BasketService basket, IShell shell)
        {
            this.basket = basket;
            this.shell = shell;

            IncrementCommand = new DelegateCommand(async p => await RunAsync(p, item => basket.IncrementAsync(item.Id)), _ => !isBusy);
            DecrementCommand = new DelegateCommand(async p => await RunAsync(p, item => basket.RemoveItemAsync(item.Id)), _ => !isBusy);
            RemoveCommand = new DelegateCommand(async p => await RunAsync(p, item => basket.RemoveItemAsync(item.Id, item.Quantity)), _ => !isBusy);
            CheckoutCommand = new DelegateCommand(_ => shell.GoCheckout(), _ => Items.Count > 0);
            ShopCommand = new DelegateCommand(_ => shell.GoShop());
        }

        public ObservableCollection<BasketItem> Items { get; } = new();

        public BasketTotals? Totals => basket.Totals;

        public bool IsEmpty => Items.Count == 0;

        public DelegateCommand IncrementCommand { get; }
        public DelegateCommand DecrementCommand { get; }
        public DelegateCommand RemoveCommand { get; }
        public DelegateCommand CheckoutCommand { get; }
        public DelegateCommand ShopCommand { get; }

        public override Task LoadAsync()
        {
            Refresh();
            return Task.CompletedTask;
        }

        private async Task RunAsync(object? parameter, Func<BasketItem, Task> action)
        {
            if (parameter is not BasketItem item) return;
            isBusy = true;
            RaiseCommandStates();
            try
            {
                await action(item);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                shell.ShowMessage(ex is ApiException apiError ? apiError.Details : ex.Message, isError: true);
            }
            finally
            {
                isBusy = false;
                Refresh();
            }
        }

        /// <summary>Rebuilds the list from the service's copy (BasketItem has no change notification of its own).</summary>
        private void Refresh()
        {
            Items.Clear();
            if (basket.Basket is { } current)
                foreach (var item in current.Items) Items.Add(item);
            RaisePropertyChanged(nameof(Totals));
            RaisePropertyChanged(nameof(IsEmpty));
            RaiseCommandStates();
        }

        private void RaiseCommandStates()
        {
            IncrementCommand.RaiseCanExecuteChanged();
            DecrementCommand.RaiseCanExecuteChanged();
            RemoveCommand.RaiseCanExecuteChanged();
            CheckoutCommand.RaiseCanExecuteChanged();
        }
    }
}
