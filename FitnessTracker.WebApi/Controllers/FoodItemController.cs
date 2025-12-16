using FitnessTracker.Application.DTOs.FoodItem;
using FitnessTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FitnessTracker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodItemsController : ControllerBase
    {
        private readonly IFoodItemService _service;
        private readonly ILogger<FoodItemsController> _logger;
        private readonly IMemoryCache _cache;

        public FoodItemsController(IFoodItemService service, ILogger<FoodItemsController> logger, IMemoryCache cache)
        {
            _service = service;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetAll(CancellationToken cancellationToken)
        {
            const string cacheKey = "FoodItems_All";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<FoodItemDto>? items))
            {
                _logger.LogInformation("Cache miss for {CacheKey}. Loading food items from database.", cacheKey);

                items = await _service.GetAllAsync(cancellationToken);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                _cache.Set(cacheKey, items, cacheOptions);
            }
            else
            {
                _logger.LogInformation("Cache hit for {CacheKey}. Returning food items from cache.", cacheKey);
            }

            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FoodItemDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var item = await _service.GetByIdAsync(id, cancellationToken);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<FoodItemDto>> Create([FromBody] CreateFoodItemDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var created = await _service.CreateAsync(dto, cancellationToken);
                _cache.Remove("FoodItems_All");
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error when creating food item.");
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<FoodItemDto>> Update(int id, [FromBody] UpdateFoodItemDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto, cancellationToken);
                if (updated == null)
                    return NotFound();
                _cache.Remove("FoodItems_All");

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error when updating food item {FoodItemId}.", id);
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
                var deleted = await _service.DeleteAsync(id, cancellationToken);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Attempted to delete food item {FoodItemId} that has logs.", id);
                return Conflict(new ProblemDetails
                {
                    Title = "Cannot delete food item",
                    Detail = ex.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }
        }
    }
}
