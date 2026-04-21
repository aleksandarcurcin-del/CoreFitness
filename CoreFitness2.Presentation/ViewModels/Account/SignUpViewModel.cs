using System.ComponentModel.DataAnnotations;

namespace CoreFitness2.Presentation.ViewModels.Account;

public class SignUpViewModel
{

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = null!;

    public string? ReturnUrl { get; set; }
    public List<string> ExternalProviders { get; set; } = [];
}
