using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WeShopAlot.Data.Models.Base;

namespace WeShopAlot.Data.Models
{
    public class MunicipalCouncilMember : BasePersistentOfficeHolder
    {
        #region Physical Properties

        [Required]
        [ForeignKey("MunicipalityId")]
        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }

        #endregion Physical Properties
    }
}
