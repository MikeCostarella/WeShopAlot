using WeShopAlot.Shared.Enumerations;
using WeShopAlot.Data.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace WeShopAlot.Data.Models
{
    public class MunicipalityType : BasePersistentObject
    {
        #region Physical Properties

        [Required]
        [StringLength(20)]
        public string Description { get; set; }

        [Required]
        [StringLength(20)]
        public MunicipalityTypeEnum Name { get; set; }

        #endregion Physical Properties
    }
}
