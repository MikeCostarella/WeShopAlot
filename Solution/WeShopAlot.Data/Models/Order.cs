using WeShopAlot.Data.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WeShopAlot.Data.Shared.Enumerations;

namespace WeShopAlot.Data.Models
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
        // OrderStatus rows are seeded with Ids 1-3, so the old default of 0 broke the foreign key
        // and every new order failed with "An error occurred while saving the entity changes".
        public int StatusId { get; set; } = (int)OrderStatusEnum.Pending;
        public OrderStatus Status { get; set; }

        // decimal(5,2) topped out at 999.99, so any larger order failed to save.
        [Column(TypeName = "decimal (18,2)")]
        public decimal Subtotal { get; set; }

        #endregion Physical Properties

        #region Child List Properties

        public IReadOnlyList<OrderItem> OrderItems { get; set; }

        #endregion Child List Properties
    }
}
