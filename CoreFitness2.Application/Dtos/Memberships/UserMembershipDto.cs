namespace CoreFitness2.Application.Dtos.Memberships;

public class UserMembershipDto
{
    public Guid Id { get; set; }
    public int MemberId { get; set; }
    public Guid MembershipPlanId { get; set; }
    public string MembershipPlanType { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime StartDate { get; set; }

}
