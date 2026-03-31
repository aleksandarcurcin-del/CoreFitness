using CoreFitness2.Domain.Entities.MembershipPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness2.Infrastructure.Configurations;

public class MembershipPlanEntityConfig : IEntityTypeConfiguration<MembershipPlanEntity>
{
    public void Configure(EntityTypeBuilder<MembershipPlanEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MembershipPlanType)
            .HasConversion<string>();

        builder.Property(x => x.Price)
            .HasPrecision(10, 2);

        builder.HasMany(x => x.Features)
            .WithOne(x => x.MembershipPlan)
            .HasForeignKey(x => x.MembershipPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
