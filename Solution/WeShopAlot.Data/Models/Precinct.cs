using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WeShopAlot.Data.Models.Base;

namespace WeShopAlot.Data.Models
{
    public class Precinct : BasePersistentObject
    {
        #region Physical Properties

        [StringLength(3)]
        public string Code { get; set; }

        [ForeignKey("CountyId")]
        public int? CountyId { get; set; }
        public County County { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string MediaMarket { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        #endregion Physical Properties
    }
}
