using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WeShopAlot.Data.Models.Base;

namespace WeShopAlot.Data.Models
{
    public class MunicipalIncomeTaxRate : BasePersistentObject
    {
        #region Physical Properties

        public DateTime? EndDate { get; set; }

        [Required]
        [ForeignKey("MunicipalityId")]
        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }

        [Column(TypeName = "decimal (5,3)")]
        public decimal Rate { get; set; }

        public DateTime StartDate { get; set; }

        #endregion Physical Properties
    }
}
