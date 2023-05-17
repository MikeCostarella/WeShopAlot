using WeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WeShopAlot.Shared.Enumerations.Counties;

namespace WeShopAlot.Data.Models
{
    public class County : BasePersistentObject
    {
        #region Physical Properties

        [Required]
        [StringLength(300)]
        public OhioCountyEnum Name { get; set; }

        [Required]
        [ForeignKey("StateId")]
        public int StateId { get; set; }
        public State State { get; set; }

        #endregion Physical Properties

        #region Child List Properties

        public List<MunicipalityCounty> Municipalities { get; set; }
        public List<Precinct> Precincts { get; set; }
        public List<Township> Townships { get; set; }

        #endregion Child List Properties
    }
}
