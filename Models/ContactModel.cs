using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class ContactModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
