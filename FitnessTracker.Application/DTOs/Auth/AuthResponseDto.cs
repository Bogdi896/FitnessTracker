using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.Application.DTOs.Auth
{
    namespace FitnessTracker.Application.DTOs.Auth
    {
        public class AuthResponseDto
        {
            public string Token { get; set; } = null!;
            public DateTime ExpiresAt { get; set; }

            public int UserId { get; set; }
            public string Username { get; set; } = null!;
            public string Role { get; set; } = null!;
        }
    }
}
