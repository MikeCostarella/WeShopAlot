using WeShopAlot.Data.Model.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeShopAlot.Data.Models
{
    public class OrderItem : BasePersistentObject
    {
        public ProductItemOrdered ItemOrdered { get; set; }

        [Column(TypeName = "decimal (5,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
