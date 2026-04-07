using CoreFitness2.Application.Dtos.Profile;
using CoreFitness2.Application.Results;

namespace CoreFitness2.Application.Interfaces;

public interface IUserAccountGateway
{
    Task<ProfileDto> GetProfileAsync(string UserId);
    Task<ServiceResult> UpdateProfileAsync(string UserId, UpdateProfileDto dto);
    Task<ServiceResult> DeleteProfileAsync(string UserId);
}
