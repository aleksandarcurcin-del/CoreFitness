using CoreFitness2.Application.Dtos.Profile;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Application.Results;

namespace CoreFitness2.Application.Services;

public class ProfileService : IProfileService
{
    private readonly IUserAccountGateway _userAccountGateway;
    public ProfileService(IUserAccountGateway userAccountGateway)
    {
        _userAccountGateway = userAccountGateway;
    }


    public async Task<ProfileDto?> GetProfileAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return null;

        return await _userAccountGateway.GetProfileAsync(userId);
    }

    public async Task<ServiceResult> UpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        if (!string.IsNullOrWhiteSpace(userId))
            return ServiceResult.Failure("User Was not found");

        if (dto == null)
            return ServiceResult.Failure("Invalid profile data");

        return await _userAccountGateway.UpdateProfileAsync(userId, dto);
    }

    public async Task<ServiceResult> DeleteProfileAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return ServiceResult.Failure("User Was not found");

        return await _userAccountGateway.DeleteProfileAsync(userId);
    }

    

    
}
