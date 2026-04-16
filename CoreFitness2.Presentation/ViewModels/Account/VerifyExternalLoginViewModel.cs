using System.ComponentModel.DataAnnotations;

namespace CoreFitness2.Presentation.ViewModels.Account;

public class VerifyExternalLoginViewModel
{
    public string Email { get; set; } = null!;
    public string? ReturnUrl { get; set; }

    [Required(ErrorMessage = "Verification code is required.")]
    public string Code { get; set; } = null!;
}
