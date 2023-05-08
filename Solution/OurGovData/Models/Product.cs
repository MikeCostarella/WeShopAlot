using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeShopAlot.Data.Models
{
    public class Product
    {
        #region Physical Properties

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public string PictureUrl { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey("ProductBrandId")]
        public int ProductBrandId { get; set; }
        public ProductBrand ProductBrand { get; set; }

        [Required]
        [ForeignKey("ProductTypeId")]
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }

        #endregion Physical Properties
    }
}
