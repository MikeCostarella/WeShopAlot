using MeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeShopAlot.Data.Models
{
    public class AppUser : BasePersistentObject
    {
        public string DisplayName { get; set; }

        [ForeignKey("AddressId")]
        public int? AddressId { get; set; }
        public Address Address { get; set; }

    }
}
