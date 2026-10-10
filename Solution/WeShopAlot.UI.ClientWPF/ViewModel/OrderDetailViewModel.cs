using WeShopAlot.UI.ClientWPF.Command;
using WeShopAlot.UI.ClientWPF.Models;
using WeShopAlot.UI.ClientWPF.Services;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>One order (Angular's OrderDetailedComponent): GET /api/order/{id}.</summary>
    public class OrderDetailViewModel : ViewModelBase
    {
        public override string? PageTitle => $"Order #{orderId}";

        public override string Breadcrumb => $"Home  /  Orders  /  Order #{orderId}";

        private readonly int orderId;
        private readonly ApiClient api;
        private readonly IShell shell;
        private Order? order;

        public OrderDetailViewModel(int orderId, ApiClient api, IShell shell)
        {
            this.orderId = orderId;
            this.api = api;
            this.shell = shell;
            BackCommand = new DelegateCommand(_ => shell.GoOrders());
        }

        public Order? Order
        {
            get => order;
            private set
            {
                if (!SetProperty(ref order, value)) return;
                RaisePropertyChanged(nameof(Heading));
                RaisePropertyChanged(nameof(ShipToName));
                RaisePropertyChanged(nameof(ShipToLines));
            }
        }

        public string Heading => Order is null ? $"Order #{orderId}" : $"Order #{Order.Id} – {Order.Status}";

        public string ShipToName => Order?.ShipToAddress is { } a ? $"{a.FirstName} {a.LastName}".Trim() : "";

        public string ShipToLines => Order?.ShipToAddress is { } a ? $"{a.Street}\n{a.City}, {a.State} {a.Zipcode}" : "";

        public DelegateCommand BackCommand { get; }

        public override async Task LoadAsync()
        {
            Order = await api.GetOrderAsync(orderId);
            if (Order is null)
            {
                shell.ShowMessage($"Order #{orderId} was not found.", isError: true);
                shell.GoOrders();
            }
        }
    }
}
