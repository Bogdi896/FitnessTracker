using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.DTOs.Auth
{
    public class RegisterUserDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly BirthDate { get; set; }

        public string Gender { get; set; } = null!;
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Role { get; set; }
    }
}
