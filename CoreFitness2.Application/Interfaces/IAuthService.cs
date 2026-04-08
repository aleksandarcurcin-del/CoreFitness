using CoreFitness2.Application.Dtos.Auth;
using CoreFitness2.Application.Results;

namespace CoreFitness2.Application.Interfaces;

public interface IAuthService
{
    Task<ServiceResult> RegisterAsync(SignUpDto dto);
    Task<ServiceResult> SignInAsync(SignInDto dto);
    Task SignOutAsync();
}