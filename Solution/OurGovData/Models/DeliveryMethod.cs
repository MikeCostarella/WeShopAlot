using MeShopAlot.Data.Model.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeShopAlot.Data.Models
{
    public class DeliveryMethod : BasePersistentObject
    {
        #region Physical Properties

        public string DeliveryTime { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal (5,2)")]
        public decimal Price { get; set; }

        public string ShortName { get; set; }

        #endregion Physical Properties
    }
}
