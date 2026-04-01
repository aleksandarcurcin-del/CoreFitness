using CoreFitness2.Application.Interfaces;
using CoreFitness2.Domain.Entities.Classes;
using CoreFitness2.Infrastructure.Data;

namespace CoreFitness2.Infrastructure.Repositories;

public class GymClassRepository(ApplicationDbContext context) : BaseRepository<GymClassEntity>(context), IGymClassRepository
{
}
