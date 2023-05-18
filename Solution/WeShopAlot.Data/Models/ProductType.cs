using WeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations;

namespace WeShopAlot.Data.Models
{
    public class ProductType : BasePersistentObject
    {
        #region Physical Properties

        public int InternalId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        #endregion Physical Properties

        #region Child List Properties

        public List<Product> Products { get; set; }

        #endregion Child List Properties
    }
}
