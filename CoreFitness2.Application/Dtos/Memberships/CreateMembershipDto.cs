namespace CoreFitness2.Application.Dtos.Memberships;

public class CreateMembershipDto
{
    public string UserId { get; set; } = null!;

    public Guid MembershipPlanId { get; set; }
}
