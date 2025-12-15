using FitnessTracker.Application.DTOs.MeasurementLog;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Application.Mappings;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infrastructure.Repositories;
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

        private const decimal MaxDailyWeightChangeKg = 3m;

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

            ValidateValues(dto.Date, dto.Weight, dto.BodyFatPercentage, dto.WaistCircumference, dto.Chest, dto.Arms);

            var existingLogs = (await _unitOfWork.MeasurementLogs.FindAsync(m => m.UserId == dto.UserId)).ToList();
            ValidateWeightChange(dto.Date, dto.Weight, existingLogs);

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

            ValidateValues(dto.Date, dto.Weight, dto.BodyFatPercentage, dto.WaistCircumference, dto.Chest, dto.Arms);

            var existingLogs = (await _unitOfWork.MeasurementLogs.FindAsync(m => m.UserId == log.UserId && m.Id != id)).ToList();
            ValidateWeightChange(dto.Date, dto.Weight, existingLogs);

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

        private static void ValidateValues(
            DateOnly date,
            decimal weight,
            decimal? bodyFatPercentage,
            decimal? waist,
            decimal? chest,
            decimal? arms)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (date > today)
                throw new ArgumentException("Measurement date cannot be in the future.", nameof(date));

            if (weight <= 0)
                throw new ArgumentException("Weight must be greater than zero.", nameof(weight));

            if (bodyFatPercentage.HasValue)
            {
                if (bodyFatPercentage.Value < 2 || bodyFatPercentage.Value > 60)
                    throw new ArgumentException("Body fat percentage must be between 2% and 60%.", nameof(bodyFatPercentage));
            }

            if (waist.HasValue)
            {
                if (waist.Value <= 0 || waist.Value < 30 || waist.Value > 200)
                    throw new ArgumentException("Waist circumference must be between 30 and 200 cm.", nameof(waist));
            }

            if (chest.HasValue)
            {
                if (chest.Value <= 0 || chest.Value < 50 || chest.Value > 200)
                    throw new ArgumentException("Chest circumference must be between 50 and 200 cm.", nameof(chest));
            }

            if (arms.HasValue)
            {
                if (arms.Value <= 0 || arms.Value < 15 || arms.Value > 80)
                    throw new ArgumentException("Arm circumference must be between 15 and 80 cm.", nameof(arms));
            }
        }

        private static void ValidateWeightChange(DateOnly newDate, decimal newWeight, List<MeasurementLog> existingLogs)
        {
            if (!existingLogs.Any())
                return;

            foreach (var log in existingLogs)
            {
                var daysDiff = Math.Abs((newDate.ToDateTime(TimeOnly.MinValue) - log.Date.ToDateTime(TimeOnly.MinValue)).TotalDays);

                if (daysDiff <= 0)
                {
                    daysDiff = 1;
                }

                var allowedChange = MaxDailyWeightChangeKg * (decimal)daysDiff;
                var actualChange = Math.Abs(newWeight - log.Weight);

                if (actualChange > allowedChange)
                {
                    throw new ArgumentException(
                        $"Weight change is too large: {actualChange:0.##} kg over {daysDiff:0.##} day(s). " +
                        $"Maximum allowed is {allowedChange:0.##} kg.",
                        nameof(newWeight));
                }
            }
        }
    }
}
