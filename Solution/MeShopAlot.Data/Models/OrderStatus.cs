using MeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations;
using MeShopAlot.Data.Shared.Enumerations;

namespace MeShopAlot.Data.Models
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
