using MeShopAlot.Data.Model.Base;

namespace MeShopAlot.Data.Models
{
    public class OrderItem : BasePersistentObject
    {
        public ProductItemOrdered ItemOrdered { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
