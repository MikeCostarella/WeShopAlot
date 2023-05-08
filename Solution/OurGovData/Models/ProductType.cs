using MeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations;

namespace MeShopAlot.Data.Models
{
    public class ProductType : BasePersistentObject
    {
        #region Physical Properties

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        #endregion Physical Properties

        #region Child List Properties

        public List<Product> Products { get; set; }

        #endregion Child List Properties
    }
}
