using MeShopAlot.Data.Model.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeShopAlot.Data.Models
{
    public class OrderItem : BasePersistentObject
    {
        public ProductItemOrdered ItemOrdered { get; set; }

        [Column(TypeName = "decimal (5,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
