using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeShopAlot.UI.ClientMVC.Models
{
    public class ProductViewModel
    {
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public int InternalId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public string PictureUrl { get; set; }

        [Required]
        [Column(TypeName = "decimal (5,2)")]
        public decimal Price { get; set; }

        public int ProductBrandId { get; set; }

        public int ProductTypeId { get; set; }
    }
}
