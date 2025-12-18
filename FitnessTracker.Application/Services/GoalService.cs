using FitnessTracker.Application.DTOs.Goal;
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
    public class GoalService : IGoalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GoalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GoalDto>> GetByUserAsync(int userId, CancellationToken cancellationToken)
        {
            var goals = await _unitOfWork.Goals.FindAsync(g => g.UserId == userId);
            return goals.Select(g => g.ToDto());
        }

        public async Task<GoalDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var goal = await _unitOfWork.Goals.GetByIdAsync(id);
            return goal?.ToDto();
        }

        public async Task<GoalDto> CreateAsync(CreateGoalDto dto, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId)
                ?? throw new ArgumentException("User not found.", nameof(dto.UserId));

            GoalValidator.ValidateGoal(dto, user);

            await GoalValidator.EnsureNoOverlap(dto.UserId, dto.GoalType, _unitOfWork, dto.StartDate, dto.EndDate);

            var entity = dto.ToEntity();
            await _unitOfWork.Goals.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task<GoalDto?> UpdateAsync(int id, UpdateGoalDto dto, CancellationToken cancellationToken)
        {
            var goal = await _unitOfWork.Goals.GetByIdAsync(id);
            if (goal == null)
                return null;

            var user = await _unitOfWork.Users.GetByIdAsync(goal.UserId)
                ?? throw new ArgumentException("User not found.", nameof(goal.UserId));

            GoalValidator.ValidateGoal(dto, user);

            await GoalValidator.EnsureNoOverlap(goal.UserId, dto.GoalType, _unitOfWork, dto.StartDate, dto.EndDate, excludeGoalId: id);

            goal.UpdateEntity(dto);
            _unitOfWork.Goals.Update(goal);
            await _unitOfWork.SaveChangesAsync();

            return goal.ToDto();
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var goal = await _unitOfWork.Goals.GetByIdAsync(id);
            if (goal == null)
                return false;

            _unitOfWork.Goals.Delete(goal);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

