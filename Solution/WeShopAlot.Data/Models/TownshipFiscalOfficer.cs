using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WeShopAlot.Data.Models.Base;

namespace WeShopAlot.Data.Models
{
    public class TownshipFiscalOfficer : BasePersistentOfficeHolder
    {
        #region Physical Properties

        [Required]
        [ForeignKey("TownshipId")]
        public int TownshipId { get; set; }
        public Township Township { get; set; }

        #endregion Physical Properties
    }
}
