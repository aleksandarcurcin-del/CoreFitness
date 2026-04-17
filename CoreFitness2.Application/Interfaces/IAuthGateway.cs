using CoreFitness2.Application.Dtos.Auth;
using CoreFitness2.Application.Models;
using CoreFitness2.Application.Results;

namespace CoreFitness2.Application.Interfaces;

public interface IAuthGateway
{
    Task<(ServiceResult Result, string? ApplicationUserId)> RegisterIdentityUserAsync(SignUpDto dto);
    Task<ServiceResult> SignInAsync(SignInDto dto);
    Task SignOutAsync();

    Task<IReadOnlyList<string>> GetExternalProvidersAsync();
    Task<ExternalUserInfo> GetExternalUserInfoAsync();
    Task<AuthenticationResult> ExternalLoginSignInAsync(ExternalUserInfo externalUserInfo, string? returnUrl = null);
    Task<(ServiceResult Result, string? ApplicationUserId)> CreateExternalIdentityUserAsync(ExternalUserInfo externalUserInfo);
    Task<ServiceResult> LinkExternalLoginAsync(string email, ExternalUserInfo externalUserInfo);
}