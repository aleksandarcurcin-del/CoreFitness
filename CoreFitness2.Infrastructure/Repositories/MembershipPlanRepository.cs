using CoreFitness2.Application.Interfaces;
using CoreFitness2.Domain.Entities.MembershipPlans;
using CoreFitness2.Infrastructure.Data;

namespace CoreFitness2.Infrastructure.Repositories;

public class MembershipPlanRepository : BaseRepository<MembershipPlanEntity>, IMembershipPlanRepository
{
    public MembershipPlanRepository(ApplicationDbContext context) : base(context)
    {
    }
}