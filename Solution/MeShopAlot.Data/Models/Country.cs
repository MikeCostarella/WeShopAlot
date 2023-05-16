using WeShopAlot.Shared.Enumerations;
using WeShopAlot.Data.Model.Base;
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
        [StringLength(300)]
        public CountryEnum Name { get; set; }

        #endregion Physical Properties

        #region Child List Properties

        public List<State> States { get; set; }

        #endregion Child List Properties
    }
}
