using CoreFitness2.Domain.Entities.MembershipPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness2.Infrastructure.Configurations;

public class MembershipPlanFeatureEntityConfig : IEntityTypeConfiguration<MembershipPlanFeatureEntity>
{
    public void Configure(EntityTypeBuilder<MembershipPlanFeatureEntity> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
