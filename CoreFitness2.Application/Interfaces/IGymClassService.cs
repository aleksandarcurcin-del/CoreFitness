using CoreFitness2.Application.Dtos.Classes;
using CoreFitness2.Application.Results;

namespace CoreFitness2.Application.Interfaces;

public interface IGymClassService
{
    Task<IEnumerable<GymClassDto>> GetAllAsync();
    Task<GymClassDto?> GetByIdAsync(int id);
    Task<ServiceResult> CreateAsync(CreateGymClassDto dto);
    Task<ServiceResult> UpdateAsync(UpdateGymClassDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
