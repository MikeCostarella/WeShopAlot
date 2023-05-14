using MeShopAlot.Shared.Enumerations;
using MeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations;

namespace MeShopAlot.Data.Models
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
