using System.ComponentModel.DataAnnotations;
using WeShopAlot.Data.Models.Base;

namespace WeShopAlot.Data.Models
{
    public class MunicipalIncomeTaxManagementAgency : BasePersistentObject
    {
        #region Physical Properties

        [StringLength(20)]
        public string Abbreviation { get; set; }

        public int InternalId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        #endregion Physical Properties
    }
}
