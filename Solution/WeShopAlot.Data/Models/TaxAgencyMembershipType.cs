using System.ComponentModel.DataAnnotations;
using WeShopAlot.Data.Models.Base;
using WeShopAlot.Data.Shared.Enumerations;

namespace WeShopAlot.Data.Models
{
    public class TaxAgencyMembershipType : BasePersistentObject
    {
        #region Physical Properties

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public TaxAgencyMembershipTypeEnum Name { get; set; }

        #endregion Physical Properties
    }
}
