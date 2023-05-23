using System.ComponentModel.DataAnnotations;

namespace WeShopAlot.Data.Models.Base
{
    public class BasePersistentOfficeHolder : BasePersistentObject
    {
        #region Physical Properties

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        public DateTime? TermEndDate { get; set; }

        public DateTime? TermStartDate { get; set; }

        #endregion Physical Properties
    }
}
