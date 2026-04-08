using CoreFitness2.Application.Interfaces;
using CoreFitness2.Domain.Entities.Members;
using CoreFitness2.Infrastructure.Data;

namespace CoreFitness2.Infrastructure.Repositories;

public class MemberRepository(ApplicationDbContext Context) : BaseRepository<MemberEntity>(Context), IMemberRepository
{
}
