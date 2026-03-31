using CoreFitness2.Application.Dtos.Memberships;

namespace CoreFitness2.Presentation.ViewModels.Membership;

public class MembershipIndexViewModel
{
    public List<MembershipPlanDto> Plans { get; set; } = [];
    public UserMembershipDto? CurrentMembership { get; set; }
}