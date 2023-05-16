using WeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations;
using WeShopAlot.Data.Shared.Enumerations;

namespace WeShopAlot.Data.Models
{
    public class OrderStatus : BasePersistentObject
    {
        #region Physical Properties

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public OrderStatusEnum Name { get; set; }

        #endregion Physical Properties
    }
}
