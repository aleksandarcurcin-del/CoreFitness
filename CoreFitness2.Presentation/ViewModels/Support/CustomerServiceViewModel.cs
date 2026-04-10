using System.ComponentModel.DataAnnotations;

namespace CoreFitness2.Presentation.ViewModels.Support
{
    public class CustomerServiceViewModel
    {

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = null!;


        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        public string Message { get; set; } = null!;


    }
}
