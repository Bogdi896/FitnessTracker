using FitnessTracker.Application.DTOs.Exercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Interfaces
{
    public interface IExerciseService
    {
        Task<IEnumerable<ExerciseDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<ExerciseDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<ExerciseDto> CreateAsync(CreateExerciseDto dto, CancellationToken cancellationToken);
        Task<ExerciseDto?> UpdateAsync(int id, UpdateExerciseDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
