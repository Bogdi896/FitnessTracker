using FitnessTracker.Application.DTOs.FoodLog;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Application.Mappings;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infrastructure.Repositories;
using FitnessTracker.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Services
{
    public class FoodLogService : IFoodLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FoodLogDto>> GetByUserAsync(int userId, CancellationToken cancellationToken)
        {
            var logs = await _unitOfWork.FoodLogs.FindAsync(fl => fl.UserId == userId);
            return logs.Select(l => l.ToDto());
        }

        public async Task<FoodLogDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var log = await _unitOfWork.FoodLogs.GetByIdAsync(id);
            return log?.ToDto();
        }

        public async Task<FoodLogDto> CreateAsync(CreateFoodLogDto dto, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId)
                ?? throw new ArgumentException("User not found.", nameof(dto.UserId));

            var food = await _unitOfWork.FoodItems.GetByIdAsync(dto.FoodId)
                ?? throw new ArgumentException("Food item not found.", nameof(dto.FoodId));

            FoodLogValidator.Validate(dto, user);

            var entity = dto.ToEntity();
            await _unitOfWork.FoodLogs.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task<FoodLogDto?> UpdateAsync(int id, UpdateFoodLogDto dto, CancellationToken cancellationToken)
        {
            var log = await _unitOfWork.FoodLogs.GetByIdAsync(id);
            if (log == null)
                return null;

            var user = await _unitOfWork.Users.GetByIdAsync(log.UserId)
                ?? throw new InvalidOperationException("User associated with log was not found.");

            FoodLogValidator.Validate(dto, user);

            log.UpdateEntity(dto);
            _unitOfWork.FoodLogs.Update(log);
            await _unitOfWork.SaveChangesAsync();

            return log.ToDto();
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var log = await _unitOfWork.FoodLogs.GetByIdAsync(id);
            if (log == null)
                return false;

            _unitOfWork.FoodLogs.Delete(log);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
