using CoreFitness2.Application.Dtos.Memberships;

namespace CoreFitness2.Application.Interfaces;

public interface IMembershipService
{

        Task<bool> CreateMembershipAsync(CreateMembershipDto dto);
        Task<List<MembershipPlanDto>> GetAllPlansAsync();

        Task<UserMembershipDto?> GetUserMembershipAsync(string userId);

        Task<bool> ChangeMembershipPlanAsync(string userId, Guid newPlanId);

        Task<bool> CancelMembershipAsync(string userId);
    

}
