using MeShopAlot.Data.Model.Base;
using System.ComponentModel.DataAnnotations;

namespace MeShopAlot.Data.Models
{
    public class Individual : BasePersistentObject
    {
        public DateTime? BirthDate { get; set; }

        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        [StringLength(30)]
        public string MiddleName { get; set; }
    }
}
