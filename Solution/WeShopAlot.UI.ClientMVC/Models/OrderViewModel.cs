using System.ComponentModel.DataAnnotations.Schema;

namespace WeShopAlot.UI.ClientMVC.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        [Column(TypeName = "decimal (5,2)")]
        public decimal OrderTotal { get; set; }

        public DateTime OrderPlaced { get; set; }

        public List<OrderDetailViewModel>? OrderDetails { get; set; }
    }
}
