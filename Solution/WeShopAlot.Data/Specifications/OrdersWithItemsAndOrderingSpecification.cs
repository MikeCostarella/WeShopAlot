using WeShopAlot.Data.Models;
using WeShopAlot.Data.Specifications.Base;

namespace WeShopAlot.Data.Specifications
{
    public class OrdersWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecification(string email) : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddIncludesForOrderDetails();
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrdersWithItemsAndOrderingSpecification(int id, string email)
            : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddIncludesForOrderDetails();
        }

        // ItemOrdered, ShipToAddress and Status live in their own tables here (in the course this app is
        // modeled on they were owned types that load automatically), so they must be included explicitly.
        // Without them, mapping an order to OrderToReturnDto threw a NullReferenceException.
        private void AddIncludesForOrderDetails()
        {
            AddInclude("OrderItems.ItemOrdered");
            AddInclude(o => o.ShipToAddress);
            AddInclude(o => o.Status);
        }
    }
}
