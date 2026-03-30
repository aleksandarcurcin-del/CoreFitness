namespace CoreFitness2.Domain.Entities.MembershipPlans;

public class MembershipPlanEntity
{
    public Guid Id { get; set; }
    public MembershipPlanType MembershipPlanType { get; set; }
    public string Description { get; set; } = null!;
    public ICollection<MembershipPlanFeatureEntity> Features { get; set; } = [];
    public decimal Price { get; set; }
    public int MonthlyClassLimit { get; set; }
    public int FreeTrialWeeks { get; set; }
    public int SortOrder { get; set; }
}
