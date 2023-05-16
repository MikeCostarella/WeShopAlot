using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeShopAlot.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        [ForeignKey("AddressId")]
        public int? AddressId { get; set; }
        public Address Address { get; set; }
    }
}
