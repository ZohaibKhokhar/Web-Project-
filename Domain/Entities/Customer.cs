using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "The name can only contain letters and spaces.")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [RegularExpression(@"^[^<>@|]+$", ErrorMessage = "The Additional Information  contains not allowed characters: <, >, @, |.")]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
