using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeShopAlot.Data.Models
{
    public class AppUser : IdentityUser
    {
        #region Physical Properties

        [ForeignKey("AddressId")]
        public int? AddressId { get; set; }
        public Address Address { get; set; }

        public string DisplayName { get; set; }

        #endregion Physical Properties
    }
}
