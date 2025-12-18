using FitnessTracker.Application.DTOs.User;
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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Select(u => u.ToDto());
        }

        public async Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user?.ToDto();
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken)
        {
            UserValidator.ValidateUser(dto);

            var existingUsers = await _unitOfWork.Users.GetAllAsync();
            if (existingUsers.Any(u => u.Email == dto.Email))
            {
                throw new ArgumentException("Email already exists.", nameof(dto.Email));
            }

            var entity = dto.ToEntity();
            await _unitOfWork.Users.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.ToDto();
        }

        public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto, CancellationToken cancellationToken)
        {
            UserValidator.ValidateUser(dto);

            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            var existingUsers = await _unitOfWork.Users.GetAllAsync();
            if (existingUsers.Any(u => u.Email == dto.Email && u.Id != id))
            {
                throw new ArgumentException("Email already exists.", nameof(dto.Email));
            }

            user.UpdateEntity(dto);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return user.ToDto();
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                return false;

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

