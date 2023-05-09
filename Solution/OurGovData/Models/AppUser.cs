using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeShopAlot.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        [ForeignKey("AddressId")]
        public int? AddressId { get; set; }
        public Address Address { get; set; }
    }
}
