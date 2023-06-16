using System.ComponentModel.DataAnnotations.Schema;
using WeShopAlot.Data.Models;

namespace WeShopAlot.UI.ClientMVC.Models
{
    public class OrderDetailViewModel
    {
        public int OrderDetailId { get; set; }

        public int ProductId { get; set; }

        public ProductViewModel? Product { get; set; }

        public int OrderId { get; set; }

        public Order? Order { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal (5,2)")]
        public decimal Price { get; set; }
    }
}
