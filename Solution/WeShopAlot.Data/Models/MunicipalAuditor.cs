using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WeShopAlot.Data.Model.Base;

namespace WeShopAlot.Data.Models
{
    public class MunicipalAuditor : BasePersistentObject
    {
        #region Physical Properties

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [ForeignKey("MunicipalityId")]
        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }

        public DateTime? TermEndDate { get; set; }

        public DateTime? TermStartDate { get; set; }

        #endregion Physical Properties
    }
}
