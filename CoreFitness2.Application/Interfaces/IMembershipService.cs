using CoreFitness2.Application.Dtos.Memberships;

namespace CoreFitness2.Application.Interfaces;

public interface IMembershipService
{

        Task<bool> CreateMembershipAsync(CreateMembershipDto dto);
        Task<List<MembershipPlanDto>> GetAllPlansAsync();

        Task<UserMembershipDto?> GetMemberMembershipAsync(int memberId);

        Task<bool> ChangeMembershipPlanAsync(int memberId, Guid newPlanId);

        Task<bool> CancelMembershipAsync(int memberId);
    

}
