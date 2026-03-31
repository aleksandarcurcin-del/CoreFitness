using CoreFitness2.Domain.Entities.MembershipPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness2.Infrastructure.Configurations;

public class MembershipEntityConfig : IEntityTypeConfiguration<MembershipEntity>
{
    public void Configure(EntityTypeBuilder<MembershipEntity> builder)
    {
        builder.ToTable("Memberships");

        builder.HasKey(x => x.UserId);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property (x => x.StartDate)
            .IsRequired();

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.HasOne(x => x.MembershipPlan)
            .WithMany()
            .HasForeignKey(x => x.MembershipPlanId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
