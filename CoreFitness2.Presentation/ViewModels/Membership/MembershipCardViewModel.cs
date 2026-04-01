using CoreFitness2.Application.Dtos.Memberships;

namespace CoreFitness2.Presentation.ViewModels.Membership;

public class MembershipCardViewModel
{
    public MembershipPlanDto Plan { get; set; } = null!;

    public UserMembershipDto? CurrentMembership { get; set; }
}
