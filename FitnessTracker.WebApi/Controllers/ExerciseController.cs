using FitnessTracker.Application.DTOs.Exercise;
using FitnessTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FitnessTracker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExercisesController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;
        private readonly ILogger<ExercisesController> _logger;
        private readonly IMemoryCache _cache;

        public ExercisesController(IExerciseService exerciseService, ILogger<ExercisesController> logger, IMemoryCache cache)
        {
            _exerciseService = exerciseService;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetAll(CancellationToken cancellationToken)
        {
            const string cacheKey = "Exercises_All";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ExerciseDto>? exercises))
            {
                _logger.LogInformation("Cache miss for {CacheKey}. Loading exercises from database.", cacheKey);

                exercises = await _exerciseService.GetAllAsync(cancellationToken);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                _cache.Set(cacheKey, exercises, cacheOptions);
            }
            else
            {
                _logger.LogInformation("Cache hit for {CacheKey}. Returning exercises from cache.", cacheKey);
            }

            return Ok(exercises);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ExerciseDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var exercise = await _exerciseService.GetByIdAsync(id, cancellationToken);
            if (exercise == null)
                return NotFound();

            return Ok(exercise);
        }

        [HttpPost]
        public async Task<ActionResult<ExerciseDto>> Create([FromBody] CreateExerciseDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var created = await _exerciseService.CreateAsync(dto, cancellationToken);
                _cache.Remove("Exercises_All");

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.Id },
                    created);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error when creating exercise.");
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ExerciseDto>> Update(int id, [FromBody] UpdateExerciseDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var updated = await _exerciseService.UpdateAsync(id, dto, cancellationToken);
                if (updated == null)
                    return NotFound();

                _cache.Remove("Exercises_All");
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error when updating exercise {ExerciseId}.", id);
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
            try
            {
                var deleted = await _exerciseService.DeleteAsync(id, cancellationToken);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // Business rule violation: used in previous workouts
                _logger.LogWarning(ex, "Attempted to delete exercise {ExerciseId} that is used in previous workouts.", id);
                return Conflict(new ProblemDetails
                {
                    Title = "Cannot delete exercise",
                    Detail = ex.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }
        }
    }
}
