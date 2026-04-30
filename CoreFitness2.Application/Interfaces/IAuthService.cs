using CoreFitness2.Application.Dtos.Auth;
using CoreFitness2.Application.Results;

namespace CoreFitness2.Application.Interfaces;

public interface IAuthService
{
    Task<ServiceResult> RegisterAsync(SignUpDto dto);
    Task<ServiceResult> SignInAsync(SignInDto dto);
    Task SignOutAsync();

    Task<IReadOnlyList<string>> GetExternalProvidersAsync();
    Task<AuthenticationResult> HandleExternalLoginCallbackAsync(string? returnUrl = null, string? remoteError = null);
    Task<AuthenticationResult> VerifyExternalLoginAsync(string code, string? returnUrl = null);
}