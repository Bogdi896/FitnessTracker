using FitnessTracker.Application.DTOs.Goal;
using FitnessTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly IGoalService _service;
        private readonly ILogger<GoalsController> _logger;

        public GoalsController(IGoalService service, ILogger<GoalsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<GoalDto>>> GetByUser(int userId, CancellationToken cancellationToken)
        {
            var goals = await _service.GetByUserAsync(userId, cancellationToken);
            return Ok(goals);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GoalDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var goal = await _service.GetByIdAsync(id, cancellationToken);
            if (goal == null)
                return NotFound();

            return Ok(goal);
        }

        [HttpPost]
        public async Task<ActionResult<GoalDto>> Create([FromBody] CreateGoalDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var created = await _service.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error creating goal.");
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation error",
                    Detail = ex.Message
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<GoalDto>> Update(int id, [FromBody] UpdateGoalDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto, cancellationToken);
                if (updated == null)
                    return NotFound();

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error updating goal.");
                return BadRequest(new ProblemDetails { Title = "Validation error", Detail = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _service.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
