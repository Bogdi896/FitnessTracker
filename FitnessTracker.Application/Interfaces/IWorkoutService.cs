using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.DTOs.Workout;

namespace FitnessTracker.Application.Interfaces
{
    public interface IWorkoutService
    {
        Task<IEnumerable<WorkoutDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<WorkoutDto>> GetByUserAsync(int userId, CancellationToken cancellationToken);
        Task<WorkoutDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<WorkoutDto> CreateAsync(CreateWorkoutDto dto, CancellationToken cancellationToken);
        Task<WorkoutDto?> UpdateAsync(int id, UpdateWorkoutDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}