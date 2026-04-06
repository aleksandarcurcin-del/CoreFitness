using CoreFitness2.Application.Dtos.Profile;
using CoreFitness2.Application.Results;

namespace CoreFitness2.Application.Interfaces;

public interface IProfileService
{
    Task<ProfileDto?> GetProfileAsync(string userId);
    Task<ServiceResult> UpdateProfileAsync(string userId, UpdateProfileDto dto);
    Task<ServiceResult> DeleteProfileAsync(string userId);
}
