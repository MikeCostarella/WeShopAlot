using System.ComponentModel.DataAnnotations;

namespace WeShopAlot.Shared.Dtos
{
    public class AddressDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Street { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        // The Address table stores a 2-character state code; anything longer used to fail in SQL Server with a 500.
        [RegularExpression("^[A-Za-z]{2}$", ErrorMessage = "State must be a 2-letter code, such as OH")]
        public string State { get; set; }

        [Required]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Zipcode must be 5 digits, or ZIP+4")]
        public string Zipcode { get; set; }
    }
}