using CoreFitness2.Domain.Entities.MembershipPlans;
using CoreFitness2.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace CoreFitness2.Infrastructure.Seeds;

public class MembershipSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.MembershipPlans.AnyAsync())
            return;

        var standardPlan = new MembershipPlanEntity
        {
            MembershipPlanType = MembershipPlanType.Standard,
            Description = "With the Standard Membership, get access to our full range of gym facilities.",
            Price = 495.00m,
            MonthlyClassLimit = 20,
            FreeTrialWeeks = 1,
            SortOrder = 1,
            Features =
            [
                new() { Description = "Standard Locker", SortOrder = 1},
                new() { Description = "High-energy group fitness classes", SortOrder = 2},
                new() { Description = "Motivating & supportive environment", SortOrder = 3}
            ]
        };

        var premiumPlan = new MembershipPlanEntity
        {
            MembershipPlanType = MembershipPlanType.Premium,
            Description = "With the Premium Membership, get access to our full range of gym facilities.",
            Price = 595.00m,
            MonthlyClassLimit = 20,
            FreeTrialWeeks = 1,
            SortOrder = 2,
            Features =
            [
                new() { Description = "Priority Support & Premium Locker", SortOrder = 1},
                new() { Description = "High-energy group fitness classes", SortOrder = 2},
                new() { Description = "Motivating & supportive environment", SortOrder = 3}
            ]
        };

        context.MembershipPlans.AddRange(standardPlan, premiumPlan);
        await context.SaveChangesAsync();
    }
}
