using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.DTOs.FoodLog;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Application.Interfaces
{
    public interface IFoodLogService
    {
        Task<IEnumerable<FoodLogDto>> GetByUserAsync(int userId, CancellationToken cancellationToken);
        Task<FoodLogDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<FoodLogDto> CreateAsync(CreateFoodLogDto dto, CancellationToken cancellationToken);
        Task<FoodLogDto?> UpdateAsync(int id, UpdateFoodLogDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
