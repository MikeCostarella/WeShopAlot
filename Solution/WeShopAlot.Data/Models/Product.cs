using WeShopAlot.Data.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeShopAlot.Data.Models
{
    public class Product : BasePersistentObject
    {
        #region Physical Properties

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
