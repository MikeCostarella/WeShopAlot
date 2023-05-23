using WeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeShopAlot.Data.Models
{
    public class Municipality : BasePersistentObject
    {
        #region Physical Properties

        public int Census2000 { get; set; }

        public int Census2010 { get; set; }

        public int Census2020 { get; set; }

        public string FormOfGovernment { get; set; }

        [StringLength(100)]
        public string MailingAddressLine1 { get; set; }

        [StringLength(100)]
        public string MailingAddressLine2 { get; set; }

        [Required]
        [ForeignKey("MunicipalityTypeId")]
        public int MunicipalityTypeId { get; set; }
        public MunicipalityType MunicipalityType { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [ForeignKey("StateId")]
        public int StateId { get; set; }
        public StateProvince State { get; set; }

        [StringLength(50)]
        public string Telephone { get; set; }

        [StringLength(250)]
        public string Website { get; set; }

        public int YearIncorporated { get; set; }

        public int ZIPCode { get; set; }

        #endregion Physical Properties

        #region Child List Properties

        public List<MunicipalAuditor> Auditors { get; set; }

        public List<MunicipalCouncilPresident> CouncilPresidents { get; set; }

        public List<MunicipalityCounty> Counties { get; set; }

        public List<MunicipalLawDirector> LawDirectors { get; set; }

        public List<Mayor> Mayors { get; set; }

        public List<MunicipalTreasurer> Treasurers { get; set; }

        #endregion Child List Properties
    }
}
