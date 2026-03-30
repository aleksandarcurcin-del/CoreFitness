using System.ComponentModel.DataAnnotations;

namespace CoreFitness2.Presentation.ViewModels.Profile;

public class ProfileViewModel
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }


    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

}
