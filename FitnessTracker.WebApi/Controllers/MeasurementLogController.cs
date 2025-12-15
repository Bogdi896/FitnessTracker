using FitnessTracker.Application.DTOs.MeasurementLog;
using FitnessTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementLogsController : ControllerBase
    {
        private readonly IMeasurementLogService _service;
        private readonly ILogger<MeasurementLogsController> _logger;

        public MeasurementLogsController(IMeasurementLogService service, ILogger<MeasurementLogsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<MeasurementLogDto>>> GetByUser(int userId, CancellationToken cancellationToken)
        {
            var logs = await _service.GetByUserAsync(userId, cancellationToken);
            return Ok(logs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MeasurementLogDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var log = await _service.GetByIdAsync(id, cancellationToken);
            if (log == null)
                return NotFound();

            return Ok(log);
        }

        [HttpPost]
        public async Task<ActionResult<MeasurementLogDto>> Create([FromBody] CreateMeasurementLogDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var created = await _service.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error when creating measurement log.");
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<MeasurementLogDto>> Update(int id, [FromBody] UpdateMeasurementLogDto dto, CancellationToken cancellationToken)
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
                _logger.LogWarning(ex, "Validation error when updating measurement log {MeasurementLogId}.", id);
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
            var deleted = await _service.DeleteAsync(id, cancellationToken);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
