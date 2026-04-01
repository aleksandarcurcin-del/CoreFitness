using CoreFitness2.Domain.Entities.Bookings;
using CoreFitness2.Domain.Entities.Classes;
using CoreFitness2.Domain.Entities.MembershipPlans;
using CoreFitness2.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness2.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<MembershipEntity> Memberships => Set<MembershipEntity>();
    public DbSet<MembershipPlanEntity> MembershipPlans => Set<MembershipPlanEntity>();
    public DbSet<MembershipPlanFeatureEntity> MembershipPlanFeatures => Set<MembershipPlanFeatureEntity>();
    public DbSet<GymClassEntity> GymClasses => Set<GymClassEntity>();
    public DbSet<BookingEntity> Bookings => Set<BookingEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(x => x.Membership)
            .WithOne()
            .HasForeignKey<MembershipEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


