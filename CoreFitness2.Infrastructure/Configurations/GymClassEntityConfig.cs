using CoreFitness2.Domain.Entities.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness2.Infrastructure.Configurations;

public class GymClassEntityConfig : IEntityTypeConfiguration<GymClassEntity>
{
    public void Configure(EntityTypeBuilder<GymClassEntity> builder)
    {
        builder.ToTable("GymClasses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Instructor)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.StartTime)
            .IsRequired();

        builder.Property(x => x.EndTime)
            .IsRequired();

        builder.Property(x => x.MaxParticipants)
            .IsRequired();
    }
}
