using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "The name can only contain letters and spaces.")]
        public string Name { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please select a pet type.")]
        [RegularExpression("^(dog|cat|bird|other)$", ErrorMessage = "Please select a valid pet type.")]
        public string PetType { get; set; }

        [Required]
        public DateTime preferredDateTime { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [RegularExpression(@"^[^<>@|]+$", ErrorMessage = "The Additional Information  contains not allowed characters: <, >, @, |.")]
        public string Reason { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
