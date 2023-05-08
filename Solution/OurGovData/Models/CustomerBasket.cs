using MeShopAlot.Data.Model.Base;

namespace MeShopAlot.Data.Models
{
    public class CustomerBasket : BasePersistentObject
    {
        #region Physical Properties

        public int? DeliveryMethodId { get; set; }

        public string ClientSecret { get; set; }

        public string PaymentIntentId { get; set; }

        public decimal ShippingPrice { get; set; }

        #endregion Physical Properties

        #region Child List Properties

        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        #endregion Child List Properties
    }
}
