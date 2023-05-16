using WeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeShopAlot.Data.Models
{
    public class Township : BasePersistentObject
    {
        #region Physical Properties

        [ForeignKey("CountyId")]
        public int? CountyId { get; set; }
        public County County { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        #endregion Physical Properties
    }
}
