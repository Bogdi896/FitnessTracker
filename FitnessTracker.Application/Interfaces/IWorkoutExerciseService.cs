using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.DTOs.WorkoutExercise;

namespace FitnessTracker.Application.Interfaces
{
    public interface IWorkoutExerciseService
    {
        Task<IEnumerable<WorkoutExerciseDto>> GetByWorkoutAsync(int workoutId, CancellationToken cancellationToken);
        Task<WorkoutExerciseDto?> GetAsync(int workoutId, int exerciseId, CancellationToken cancellationToken);
        Task<WorkoutExerciseDto> CreateAsync(CreateWorkoutExerciseDto dto, CancellationToken cancellationToken);
        Task<WorkoutExerciseDto?> UpdateAsync(int workoutId, int exerciseId, UpdateWorkoutExerciseDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int workoutId, int exerciseId, CancellationToken cancellationToken);
    }
}
