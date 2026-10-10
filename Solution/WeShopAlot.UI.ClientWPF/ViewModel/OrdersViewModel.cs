using System.Collections.ObjectModel;
using WeShopAlot.UI.ClientWPF.Command;
using WeShopAlot.UI.ClientWPF.Models;
using WeShopAlot.UI.ClientWPF.Services;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>The signed-in user's orders (Angular's OrdersComponent): GET /api/order.</summary>
    public class OrdersViewModel : ViewModelBase
    {
        public override string? PageTitle => "Orders";

        private readonly ApiClient api;
        private bool isLoading;

        public OrdersViewModel(ApiClient api, IShell shell)
        {
            this.api = api;
            ViewOrderCommand = new DelegateCommand(p => { if (p is Order order) shell.GoOrder(order.Id); });
            ShopCommand = new DelegateCommand(_ => shell.GoShop());
        }

        public ObservableCollection<Order> Orders { get; } = new();

        public bool IsLoading
        {
            get => isLoading;
            private set => SetProperty(ref isLoading, value);
        }

        public bool IsEmpty => !IsLoading && Orders.Count == 0;

        public DelegateCommand ViewOrderCommand { get; }
        public DelegateCommand ShopCommand { get; }

        public override async Task LoadAsync()
        {
            IsLoading = true;
            try
            {
                Orders.Clear();
                foreach (var order in (await api.GetOrdersAsync()).OrderByDescending(o => o.OrderDate)) Orders.Add(order);
            }
            finally
            {
                IsLoading = false;
                RaisePropertyChanged(nameof(IsEmpty));
            }
        }
    }
}
