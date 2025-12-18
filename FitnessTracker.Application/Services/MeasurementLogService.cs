using FitnessTracker.Application.DTOs.MeasurementLog;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Application.Mappings;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Services
{
    public class MeasurementLogService : IMeasurementLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MeasurementLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MeasurementLogDto>> GetByUserAsync(int userId, CancellationToken cancellationToken)
        {
            var logs = await _unitOfWork.MeasurementLogs.FindAsync(m => m.UserId == userId);
            return logs.OrderBy(l => l.Date).Select(l => l.ToDto());
        }

        public async Task<MeasurementLogDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var log = await _unitOfWork.MeasurementLogs.GetByIdAsync(id);
            return log?.ToDto();
        }

        public async Task<MeasurementLogDto> CreateAsync(CreateMeasurementLogDto dto, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId)
                ?? throw new ArgumentException("User not found.", nameof(dto.UserId));

            MeasurementLogValidator.ValidateValues(dto.Date, dto.Weight, dto.BodyFatPercentage, dto.WaistCircumference, dto.Chest, dto.Arms);

            var existingLogs = (await _unitOfWork.MeasurementLogs.FindAsync(m => m.UserId == dto.UserId)).ToList();
            MeasurementLogValidator.ValidateWeightChange(dto.Date, dto.Weight, existingLogs);

            var entity = dto.ToEntity();
            await _unitOfWork.MeasurementLogs.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task<MeasurementLogDto?> UpdateAsync(int id, UpdateMeasurementLogDto dto, CancellationToken cancellationToken)
        {
            var log = await _unitOfWork.MeasurementLogs.GetByIdAsync(id);
            if (log == null)
                return null;

            var user = await _unitOfWork.Users.GetByIdAsync(log.UserId)
                ?? throw new InvalidOperationException("User associated with the measurement log no longer exists.");

            MeasurementLogValidator.ValidateValues(dto.Date, dto.Weight, dto.BodyFatPercentage, dto.WaistCircumference, dto.Chest, dto.Arms);

            var existingLogs = (await _unitOfWork.MeasurementLogs.FindAsync(m => m.UserId == log.UserId && m.Id != id)).ToList();
            MeasurementLogValidator.ValidateWeightChange(dto.Date, dto.Weight, existingLogs);

            log.UpdateEntity(dto);
            _unitOfWork.MeasurementLogs.Update(log);
            await _unitOfWork.SaveChangesAsync();

            return log.ToDto();
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var log = await _unitOfWork.MeasurementLogs.GetByIdAsync(id);
            if (log == null)
                return false;

            _unitOfWork.MeasurementLogs.Delete(log);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
