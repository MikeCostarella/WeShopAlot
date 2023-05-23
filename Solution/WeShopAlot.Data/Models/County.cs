using WeShopAlot.Data.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeShopAlot.Data.Models
{
    public class County : BasePersistentObject
    {
        #region Physical Properties

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [ForeignKey("StateProvinceId")]
        public int StateProvinceId { get; set; }
        public StateProvince StateProvince { get; set; }

        #endregion Physical Properties

        #region Child List Properties

        public List<MunicipalityCounty> Municipalities { get; set; }
        public List<Precinct> Precincts { get; set; }
        public List<Township> Townships { get; set; }

        #endregion Child List Properties
    }
}
