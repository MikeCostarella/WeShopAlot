using WeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeShopAlot.Data.Models
{
    public class State : BasePersistentObject
    {
        #region Physical Properties

        [Required]
        [StringLength(2)]
        public string Abbreviation { get; set; }

        [Required]
        [ForeignKey("CountryId")]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        #endregion Physical Properties

        #region Child List Properties

        public List<County> Counties { get; set; }

        public List<Municipality> Municipalities { get; set; }

        #endregion Child List Properties
    }
}
