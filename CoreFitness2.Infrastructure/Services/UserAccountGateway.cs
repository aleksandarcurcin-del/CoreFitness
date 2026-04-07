using CoreFitness2.Application.Dtos.Profile;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Application.Results;
using CoreFitness2.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace CoreFitness2.Infrastructure.Services;

public class UserAccountGateway : IUserAccountGateway
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserAccountGateway(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


    public async Task<ProfileDto?> GetProfileAsync(string UserId)
    {
        var user = await _userManager.FindByIdAsync(UserId);
        if (user == null)
            return null;

        return new ProfileDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task<ServiceResult> UpdateProfileAsync(string UserId, UpdateProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(UserId);
        if (user == null)
            return ServiceResult.Failure("User not found.");


        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.UserName = dto.Email;
        user.PhoneNumber = dto.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);  

        if (result.Succeeded)
            return ServiceResult.Success();

        var firstError = result.Errors.FirstOrDefault()?.Description ?? "Profile update failed.";
        return ServiceResult.Failure(firstError);
    }


    public async Task<ServiceResult> DeleteProfileAsync(string UserId)
    {
        var user = await _userManager.FindByIdAsync(UserId);
        if (user == null)
            return ServiceResult.Failure("User not found.");

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
            return ServiceResult.Success();

        var firstError = result.Errors.FirstOrDefault()?.Description ?? "Profile deletion failed.";
        return ServiceResult.Failure(firstError);
    }

    
}
