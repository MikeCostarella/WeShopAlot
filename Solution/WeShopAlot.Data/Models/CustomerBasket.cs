using WeShopAlot.Data.Model.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeShopAlot.Data.Models
{
    public class CustomerBasket : BasePersistentObject
    {
        #region Physical Properties

        public int? DeliveryMethodId { get; set; }

        public string ClientSecret { get; set; }

        public string PaymentIntentId { get; set; }

        [Column(TypeName = "decimal (5,2)")]
        public decimal ShippingPrice { get; set; }

        #endregion Physical Properties

        #region Child List Properties

        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        #endregion Child List Properties
    }
}
