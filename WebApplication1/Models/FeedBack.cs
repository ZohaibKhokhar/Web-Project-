using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class FeedBack
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "The name can only contain letters and spaces.")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }

    }
}
