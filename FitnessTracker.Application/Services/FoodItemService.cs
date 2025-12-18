using FitnessTracker.Application.DTOs.FoodItem;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Application.Mappings;
using FitnessTracker.Infrastructure.Repositories;
using FitnessTracker.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FitnessTracker.Application.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FoodItemDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.FoodItems.GetAllAsync();
            return items.Select(i => i.ToDto());
        }

        public async Task<FoodItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.FoodItems.GetByIdAsync(id);
            return item?.ToDto();
        }

        public async Task<FoodItemDto> CreateAsync(CreateFoodItemDto dto, CancellationToken cancellationToken)
        {
            FoodItemValidator.Validate(dto);
            var existing = await _unitOfWork.FoodItems.FindAsync(f =>
                f.Name.ToLower() == dto.Name.ToLower());

            if (existing.Any())
                throw new ArgumentException("A food item with this name already exists.", nameof(dto.Name));

            var entity = dto.ToEntity();
            await _unitOfWork.FoodItems.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task<FoodItemDto?> UpdateAsync(int id, UpdateFoodItemDto dto, CancellationToken cancellationToken)
        {
            FoodItemValidator.Validate(dto);

            var item = await _unitOfWork.FoodItems.GetByIdAsync(id);
            if (item == null)
                return null;

            var existing = await _unitOfWork.FoodItems.FindAsync(f =>
                f.Id != id && f.Name.ToLower() == dto.Name.ToLower());

            if (existing.Any())
                throw new ArgumentException("A food item with this name already exists.", nameof(dto.Name));

            item.UpdateEntity(dto);
            _unitOfWork.FoodItems.Update(item);
            await _unitOfWork.SaveChangesAsync();

            return item.ToDto();
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.FoodItems.GetByIdAsync(id);
            if (item == null)
                return false;

            var logs = await _unitOfWork.FoodLogs.FindAsync(fl => fl.FoodId == id);
            if (logs.Any())
            {
                throw new InvalidOperationException("Cannot delete food item that is referenced by food logs.");
            }

            _unitOfWork.FoodItems.Delete(item);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
