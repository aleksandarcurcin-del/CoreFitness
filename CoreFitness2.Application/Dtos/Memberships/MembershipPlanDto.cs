namespace CoreFitness2.Application.Dtos.Memberships;

public class MembershipPlanDto
{
    public Guid Id { get; set; }
    public string MembershipPlanType { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int MonthlyClassLimit { get; set; }
    public int FreeTrialWeeks { get; set; }
    public List<MembershipPlanFeatureDto> Features { get; set; } = [];
}
