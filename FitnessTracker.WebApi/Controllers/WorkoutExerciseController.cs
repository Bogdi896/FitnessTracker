using FitnessTracker.Application.DTOs.WorkoutExercise;
using FitnessTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutExercisesController : ControllerBase
    {
        private readonly IWorkoutExerciseService _service;
        private readonly ILogger<WorkoutExercisesController> _logger;

        public WorkoutExercisesController(IWorkoutExerciseService service, ILogger<WorkoutExercisesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/workoutexercises?workoutId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutExerciseDto>>> GetByWorkout([FromQuery] int workoutId, CancellationToken cancellationToken)
        {
            if (workoutId <= 0)
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation error",
                    Detail = "WorkoutId must be provided and positive.",
                    Status = StatusCodes.Status400BadRequest
                });

            var items = await _service.GetByWorkoutAsync(workoutId, cancellationToken);
            return Ok(items);
        }

        // GET: api/workoutexercises/{workoutId}/{exerciseId}
        [HttpGet("{workoutId:int}/{exerciseId:int}")]
        public async Task<ActionResult<WorkoutExerciseDto>> Get(int workoutId, int exerciseId, CancellationToken cancellationToken)
        {
            var item = await _service.GetAsync(workoutId, exerciseId, cancellationToken);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST: api/workoutexercises
        [HttpPost]
        public async Task<ActionResult<WorkoutExerciseDto>> Create([FromBody] CreateWorkoutExerciseDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var created = await _service.CreateAsync(dto, cancellationToken);

                return CreatedAtAction(
                    nameof(Get),
                    new { workoutId = created.WorkoutId, exerciseId = created.ExerciseId },
                    created);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error when creating workout-exercise.");
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        // PUT: api/workoutexercises/{workoutId}/{exerciseId}
        [HttpPut("{workoutId:int}/{exerciseId:int}")]
        public async Task<ActionResult<WorkoutExerciseDto>> Update(int workoutId, int exerciseId, [FromBody] UpdateWorkoutExerciseDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var updated = await _service.UpdateAsync(workoutId, exerciseId, dto, cancellationToken);
                if (updated == null)
                    return NotFound();

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error when updating workout-exercise {WorkoutId}-{ExerciseId}.", workoutId, exerciseId);
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        // DELETE: api/workoutexercises/{workoutId}/{exerciseId}
        [HttpDelete("{workoutId:int}/{exerciseId:int}")]
        public async Task<IActionResult> Delete(int workoutId, int exerciseId, CancellationToken cancellationToken)
        {
            var deleted = await _service.DeleteAsync(workoutId, exerciseId, cancellationToken);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
