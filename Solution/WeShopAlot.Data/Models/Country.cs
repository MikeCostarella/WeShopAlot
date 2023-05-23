using WeShopAlot.Data.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace WeShopAlot.Data.Models
{
    public class Country : BasePersistentObject
    {
        #region Physical Properties

        [Required]
        [StringLength(3)]
        public string Abbreviation { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        #endregion Physical Properties

        #region Child List Properties

        public List<StateProvince> StateProvinces { get; set; }

        #endregion Child List Properties
    }
}
