using WeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeShopAlot.Data.Models
{
    public class Address : BasePersistentObject
    {
        #region Physical Properties

        [Required]
        [Display(Name = "Address Line 1")]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [Required]
        [Display(Name = "Address Line 2")]
        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [ForeignKey("CountryId")]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        [ForeignKey("CountyId")]
        public int? CountyId { get; set; }
        public County County { get; set; }

        [StringLength(10)]
        public string ZipCode { get; set; }

        #endregion Physical Properties
    }
}
