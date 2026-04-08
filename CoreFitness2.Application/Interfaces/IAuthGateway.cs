using CoreFitness2.Application.Dtos.Auth;
using CoreFitness2.Application.Results;

namespace CoreFitness2.Application.Interfaces;

public interface IAuthGateway
{
    Task<(ServiceResult Result, string? ApplicationUserId)> RegisterIdentityUserAsync(SignUpDto dto);
    Task<ServiceResult> SignInAsync(SignInDto dto);
    Task SignOutAsync();
}