using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.DTOs.MeasurementLog;

namespace FitnessTracker.Application.Interfaces
{
    public interface IMeasurementLogService
    {
        Task<IEnumerable<MeasurementLogDto>> GetByUserAsync(int userId, CancellationToken cancellationToken);
        Task<MeasurementLogDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<MeasurementLogDto> CreateAsync(CreateMeasurementLogDto dto, CancellationToken cancellationToken);
        Task<MeasurementLogDto?> UpdateAsync(int id, UpdateMeasurementLogDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
