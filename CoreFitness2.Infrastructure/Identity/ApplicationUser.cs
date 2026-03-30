using CoreFitness2.Domain.Entities.MembershipPlans;
using Microsoft.AspNetCore.Identity;

namespace CoreFitness2.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    
    public string? FirstName { get; set; }

    
    public string? LastName { get; set; }

    public MembershipPlanEntity? MembershipPlan { get; set; }
}
