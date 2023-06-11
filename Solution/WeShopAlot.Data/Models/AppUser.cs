using System.ComponentModel.DataAnnotations.Schema;
using WeShopAlot.Data.Models.Base;

namespace WeShopAlot.Data.Models
{
    public class AppUser : BasePersistentObject
    {
        #region Physical Properties

        [ForeignKey("AddressId")]
        public int? AddressId { get; set; }
        public Address Address { get; set; }

        public string DisplayName { get; set; }

        public string EmailAddress { get; set; }

        public string PasswordHash { get; set; }

        #endregion Physical Properties
    }
}
