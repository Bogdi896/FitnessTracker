using FitnessTracker.Application.DTOs.User;
using FitnessTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all users.");

            var users = await _userService.GetAllAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} users.", users.Count());

            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetById(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", id);

            var user = await _userService.GetByIdAsync(id, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved user with ID {UserId}", id);

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to create a new user with email {Email}.", dto.Email);

            try
            {
                var created = await _userService.CreateAsync(dto, cancellationToken);

                _logger.LogInformation(
                    "Successfully created user {UserId} with email {Email}.",
                    created.Id, created.Email);

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex,
                    "Validation error when creating user with email {Email}.",
                    dto.Email);

                return BadRequest(new ProblemDetails
                {
                    Title = "Validation error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating user {UserId}", id);

            try
            {
                var updated = await _userService.UpdateAsync(id, dto, cancellationToken);

                if (updated == null)
                {
                    _logger.LogWarning("User {UserId} not found during update.", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully updated user {UserId}", id);

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex,
                    "Validation error when updating user {UserId}.",
                    id);

                return BadRequest(new ProblemDetails
                {
                    Title = "Validation error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request received to delete user {UserId}", id);

            var deleted = await _userService.DeleteAsync(id, cancellationToken);

            if (!deleted)
            {
                _logger.LogWarning("User {UserId} not found for deletion.", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully deleted user {UserId}", id);

            return NoContent();
        }
    }
}
