using CoreFitness2.Application.Dtos.Auth;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Application.Models;
using CoreFitness2.Application.Results;
using CoreFitness2.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

    public async Task<IReadOnlyList<string>> GetExternalProvidersAsync()
    {
        var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
        return schemes.Select(x => x.Name).ToList();
    }

    public async Task<ExternalUserInfo?> GetExternalUserInfoAsync()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
            return null;

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return new ExternalUserInfo(
            info.LoginProvider,
            info.ProviderKey,
            email
        );
    }

    public async Task<AuthenticationResult> ExternalLoginSignInAsync(ExternalUserInfo externalUserInfo, string? returnUrl = null)
    {
        var result = await _signInManager.ExternalLoginSignInAsync(
            externalUserInfo.LoginProvider,
            externalUserInfo.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true
        );

        if (result.Succeeded)
            return AuthenticationResult.SignedIn(returnUrl);

        return AuthenticationResult.RequiresVerification(externalUserInfo.Email, returnUrl);
    }

    public async Task<(ServiceResult Result, string? ApplicationUserId)> CreateExternalIdentityUserAsync(ExternalUserInfo externalUserInfo)
    {
        var user = new ApplicationUser
        {
            UserName = externalUserInfo.Email,
            Email = externalUserInfo.Email,
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(user);
        if (!createResult.Succeeded)
        {
            var firstError = createResult.Errors.FirstOrDefault()?.Description ?? "Failed to create external user.";
            return (ServiceResult.Failure(firstError), null);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "Member");
        if (!roleResult.Succeeded)
        {
            var firstError = roleResult.Errors.FirstOrDefault()?.Description ?? "Failed to assign Member role.";
            await _userManager.DeleteAsync(user);
            return (ServiceResult.Failure(firstError), null);
        }

        var loginResult = await _userManager.AddLoginAsync(
            user,
            new UserLoginInfo(
                externalUserInfo.LoginProvider,
                externalUserInfo.ProviderKey,
                externalUserInfo.LoginProvider)
        );

        if (!loginResult.Succeeded)
        {
            var firstError = loginResult.Errors.FirstOrDefault()?.Description ?? "Failed to link external login.";
            await _userManager.DeleteAsync(user);
            return (ServiceResult.Failure(firstError), null);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);

        return (ServiceResult.Success(), user.Id);
    }

    public async Task<ServiceResult> LinkExternalLoginAsync(string email, ExternalUserInfo externalUserInfo)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return ServiceResult.Failure("User not found.");

        var result = await _userManager.AddLoginAsync(
            user,
            new UserLoginInfo(
                externalUserInfo.LoginProvider,
                externalUserInfo.ProviderKey,
                externalUserInfo.LoginProvider)
        );

        if (!result.Succeeded)
        {
            var firstError = result.Errors.FirstOrDefault()?.Description ?? "Failed to link external login.";
            return ServiceResult.Failure(firstError);
        }

        if (!await _userManager.IsInRoleAsync(user, "Member"))
        {
            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded)
            {
                var firstError = roleResult.Errors.FirstOrDefault()?.Description ?? "Failed to assign Member role.";
                return ServiceResult.Failure(firstError);
            }
        }

        await _signInManager.SignInAsync(user, isPersistent: false);

        return ServiceResult.Success();
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