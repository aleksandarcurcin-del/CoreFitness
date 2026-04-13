using CoreFitness2.Application.Dtos.Auth;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Application.Results;
using CoreFitness2.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace CoreFitness2.Infrastructure.Services;

public class AuthGateway : IAuthGateway
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthGateway(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<(ServiceResult Result, string? ApplicationUserId)> RegisterIdentityUserAsync(SignUpDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var firstError = result.Errors.FirstOrDefault()?.Description ?? "Registration failed.";
            return (ServiceResult.Failure(firstError), null);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "Member");

        if (!roleResult.Succeeded)
            return (ServiceResult.Failure("Failed to assign role"), null);

        await _signInManager.SignInAsync(user, false);

        return (ServiceResult.Success(), user.Id);
    }

    public async Task<ServiceResult> SignInAsync(SignInDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(
            dto.Email,
            dto.Password,
            false,
            false);

        if (result.Succeeded)
            return ServiceResult.Success();

        return ServiceResult.Failure("Invalid login attempt.");
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}