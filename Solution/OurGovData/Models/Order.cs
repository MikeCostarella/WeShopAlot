using MeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MeShopAlot.Data.Models
{
    public class Order : BasePersistentObject
    {
        #region Physical Properties

        public string BuyerEmail { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("DeliveryMethodId")]
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        public string PaymentIntentId { get; set; }

        [Required]
        [ForeignKey("ShipToAddressId")]
        public int ShipToAddressId { get; set; }
        public Address ShipToAddress { get; set; }

        [Required]
        [ForeignKey("StatusId")]
        public int StatusId { get; set; }
        public OrderStatus Status { get; set; }

        [Column(TypeName = "decimal (5,2)")]
        public decimal Subtotal { get; set; }

        #endregion Physical Properties

        #region Child List Properties

        public IReadOnlyList<OrderItem> OrderItems { get; set; }

        #endregion Child List Properties
    }
}
