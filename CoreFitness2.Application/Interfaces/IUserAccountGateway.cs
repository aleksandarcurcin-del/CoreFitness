using CoreFitness2.Application.Results;

namespace CoreFitness2.Application.Interfaces;

public interface IUserAccountGateway
{
    Task<ServiceResult> DeleteIdentityUserAsync(string applicationUserId);
}