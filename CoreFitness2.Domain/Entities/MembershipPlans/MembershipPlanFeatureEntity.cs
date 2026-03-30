namespace CoreFitness2.Domain.Entities.MembershipPlans;

public class MembershipPlanFeatureEntity
{

    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public int SortOrder { get; set; }
    public Guid MembershipPlanId { get; set; }
}
