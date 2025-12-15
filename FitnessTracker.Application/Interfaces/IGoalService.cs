using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.DTOs.Goal;

namespace FitnessTracker.Application.Interfaces
{
    public interface IGoalService
    {
        Task<IEnumerable<GoalDto>> GetByUserAsync(int userId, CancellationToken cancellationToken);
        Task<GoalDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<GoalDto> CreateAsync(CreateGoalDto dto, CancellationToken cancellationToken);
        Task<GoalDto?> UpdateAsync(int id, UpdateGoalDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
