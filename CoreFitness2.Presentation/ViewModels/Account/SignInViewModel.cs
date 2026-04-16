using System.ComponentModel.DataAnnotations;

namespace CoreFitness2.Presentation.ViewModels.Account;

public class SignInViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public string? ReturnUrl { get; set; }
    public List<string> ExternalProviders { get; set; } = [];
}
