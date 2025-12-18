using FitnessTracker.Application.DTOs.FoodLog;
using FitnessTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class FoodLogsController : ControllerBase
    {
        private readonly IFoodLogService _service;
        private readonly ILogger<FoodLogsController> _logger;

        public FoodLogsController(IFoodLogService service, ILogger<FoodLogsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<FoodLogDto>>> GetByUser(int userId, CancellationToken cancellationToken)
        {
            var logs = await _service.GetByUserAsync(userId, cancellationToken);
            return Ok(logs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FoodLogDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var log = await _service.GetByIdAsync(id, cancellationToken);
            if (log == null)
                return NotFound();

            return Ok(log);
        }

        [HttpPost]
        public async Task<ActionResult<FoodLogDto>> Create([FromBody] CreateFoodLogDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var created = await _service.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation error",
                    Detail = ex.Message
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<FoodLogDto>> Update(int id, [FromBody] UpdateFoodLogDto dto, CancellationToken cancellationToken)
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
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation error",
                    Detail = ex.Message
                });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var success = await _service.DeleteAsync(id, cancellationToken);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
