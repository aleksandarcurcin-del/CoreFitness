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

    public async Task<ServiceResult> DeleteIdentityUserAsync(string applicationUserId)
    {
        var user = await _userManager.FindByIdAsync(applicationUserId);
        if (user == null)
            return ServiceResult.Failure("Identity user was not found.");

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
            return ServiceResult.Success();

        var firstError = result.Errors.FirstOrDefault()?.Description ?? "Could not delete identity user.";
        return ServiceResult.Failure(firstError);
    }
}