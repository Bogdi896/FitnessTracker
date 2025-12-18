using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessTracker.Application.DTOs.User;

namespace FitnessTracker.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken);
        Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
