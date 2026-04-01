using CoreFitness2.Domain.Entities.Bookings;
using CoreFitness2.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitness2.Infrastructure.Configurations;

public class BookingEntityConfig : IEntityTypeConfiguration<BookingEntity>
{
    public void Configure(EntityTypeBuilder<BookingEntity> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.GymClassId)
            .IsRequired();

        builder.Property(x => x.BookedAt)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.GymClass)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.GymClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.UserId, x.GymClassId })
            .IsUnique();
    }
}
