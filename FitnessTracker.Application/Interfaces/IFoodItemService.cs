using FitnessTracker.Application.DTOs.FoodItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Interfaces
{
    public interface IFoodItemService
    {
        Task<IEnumerable<FoodItemDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<FoodItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<FoodItemDto> CreateAsync(CreateFoodItemDto dto, CancellationToken cancellationToken);
        Task<FoodItemDto?> UpdateAsync(int id, UpdateFoodItemDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
