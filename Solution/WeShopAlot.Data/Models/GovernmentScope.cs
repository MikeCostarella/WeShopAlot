using System.ComponentModel.DataAnnotations;
using WeShopAlot.Data.Shared.Enumerations;
using WeShopAlot.Data.Model.Base;

namespace WeShopAlot.Data.Models
{
    public class GovernmentScope : BasePersistentObject
    {
        #region Physical Properties

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public GovernmentScopeEnum Name { get; set; }

        #endregion Physical Properties
    }
}
