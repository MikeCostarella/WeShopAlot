using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WeShopAlot.Data.Models.Base;

namespace WeShopAlot.Data.Models
{
    public class MunicipalityTaxAgencyRelationship : BasePersistentObject
    {
        #region Physical Properties

        public DateTime? EndDate { get; set; }

        [Required]
        [ForeignKey("MunicipalityId")]
        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }

        [Required]
        [ForeignKey("MunicipalIncomeTaxManagementAgencyId")]
        public int MunicipalIncomeTaxManagementAgencyId { get; set; }
        public MunicipalIncomeTaxManagementAgency MunicipalIncomeTaxManagementAgency { get; set; }

        [Required]
        [ForeignKey("TaxAgencyMembershipTypeId")]
        public int TaxAgencyMembershipTypeId { get; set; }
        public TaxAgencyMembershipType TaxAgencyMembershipType { get; set; }

        public DateTime StartDate { get; set; }

        #endregion Physical Properties
    }
}
