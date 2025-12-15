using FitnessTracker.Application.DTOs.Workout;
using FitnessTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;
        private readonly ILogger<WorkoutsController> _logger;

        public WorkoutsController(IWorkoutService workoutService, ILogger<WorkoutsController> logger)
        {
            _workoutService = workoutService;
            _logger = logger;
        }

        // GET: api/workouts
        // Optional: /api/workouts?userId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutDto>>> GetAll([FromQuery] int? userId, CancellationToken cancellationToken)
        {
            if (userId.HasValue)
            {
                var userWorkouts = await _workoutService.GetByUserAsync(userId.Value, cancellationToken);
                return Ok(userWorkouts);
            }

            var workouts = await _workoutService.GetAllAsync(cancellationToken);
            return Ok(workouts);
        }

        // GET: api/workouts/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<WorkoutDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var workout = await _workoutService.GetByIdAsync(id, cancellationToken);
            if (workout == null)
                return NotFound();

            return Ok(workout);
        }

        // POST: api/workouts
        [HttpPost]
        public async Task<ActionResult<WorkoutDto>> Create([FromBody] CreateWorkoutDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var created = await _workoutService.CreateAsync(dto, cancellationToken);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.Id },
                    created);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error when creating workout.");
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        // PUT: api/workouts/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<WorkoutDto>> Update(int id, [FromBody] UpdateWorkoutDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var updated = await _workoutService.UpdateAsync(id, dto, cancellationToken);
                if (updated == null)
                    return NotFound();

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error when updating workout {WorkoutId}.", id);
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        // DELETE: api/workouts/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _workoutService.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
