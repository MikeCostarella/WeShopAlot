using WeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeShopAlot.Data.Models
{
    public class Municipality : BasePersistentObject
    {
        #region Physical Properties

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [ForeignKey("MunicipalityTypeId")]
        public int MunicipalityTypeId { get; set; }
        public MunicipalityType MunicipalityType { get; set; }

        [Required]
        [ForeignKey("StateId")]
        public int StateId { get; set; }
        public StateProvince State { get; set; }

        #endregion Physical Properties

        #region Child List Properties

        public List<MunicipalityCounty> Counties { get; set; }

        #endregion Child List Properties
    }
}
