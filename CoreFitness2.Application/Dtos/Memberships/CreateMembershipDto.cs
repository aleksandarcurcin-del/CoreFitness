namespace CoreFitness2.Application.Dtos.Memberships;

public class CreateMembershipDto
{
    public int MemberId { get; set; }

    public Guid MembershipPlanId { get; set; }
}
