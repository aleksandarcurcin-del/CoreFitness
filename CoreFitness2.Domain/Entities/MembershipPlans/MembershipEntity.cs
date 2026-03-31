namespace CoreFitness2.Domain.Entities.MembershipPlans;

public class MembershipEntity
{
    public Guid Guid { get; set; }

    public string UserId { get; set; } = null!;
    public Guid MembershipPlanId { get; set; }

    public DateTime StartDate { get; set; }

    public string Status { get; set; } = null!;

    public MembershipPlanEntity MembershipPlan { get; set; } = null!;
}
