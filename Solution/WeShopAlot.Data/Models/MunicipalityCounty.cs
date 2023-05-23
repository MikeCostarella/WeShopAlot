using WeShopAlot.Data.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeShopAlot.Data.Models
{
    public class MunicipalityCounty : BasePersistentObject
    {
        #region Physical Properties

        [Required]
        [ForeignKey("CountyId")]
        public int CountyId { get; set; }
        public County County { get; set; }

        [Required]
        [ForeignKey("MunicipalityId")]
        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }

        #endregion Physical Properties
    }
}
