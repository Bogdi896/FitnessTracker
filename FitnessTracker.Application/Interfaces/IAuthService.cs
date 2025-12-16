using FitnessTracker.Application.DTOs.Auth;
using FitnessTracker.Application.DTOs.Auth.FitnessTracker.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto, CancellationToken cancellationToken);
        Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
    }
}
