using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeShopAlot.Data.Model.Base;

namespace WeShopAlot.Data.Models
{
    public class TownshipTrustee : BasePersistentObject
    {
        #region Physical Properties

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        public DateTime TermEndDate { get; set; }

        public DateTime? TermStartDate { get; set; }

        [Required]
        [ForeignKey("TownshipId")]
        public int TownshipId { get; set; }
        public Township Township { get; set; }

        #endregion Physical Properties
    }
}
