using Microsoft.AspNetCore.Mvc;
using RouteScheduler.Core.Application.Abstraction.DTOs.Drivers;
using RouteScheduler.Core.Application.Abstraction.Services;
using RouteScheduler.Core.Application.Abstraction.Common;

namespace RouteScheduler.APIs.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;

        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        /// <summary>
        /// Get all drivers
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverToReturnDto>>> GetAll()
        {
            var drivers = await _driverService.GetAllAsync();
            return Ok(drivers);
        }

        /// <summary>
        /// Get drivers with pagination
        /// </summary>
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<DriverToReturnDto>>> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _driverService.GetPagedAsync(new PaginationParams { PageNumber = pageNumber, PageSize = pageSize });
            return Ok(result);
        }

        /// <summary>
        /// Get driver by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverToReturnDto>> GetById(int id)
        {
            var driver = await _driverService.GetByIdAsync(id);
            if (driver == null)
                return NotFound($"Driver with ID {id} not found.");
            
            return Ok(driver);
        }

        /// <summary>
        /// Create a new driver
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<DriverToReturnDto>> Create([FromBody] DriverToCreateDto driverToCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var driver = await _driverService.CreateAsync(driverToCreate);
            return CreatedAtAction(nameof(GetById), new { id = driver.Id }, driver);
        }

        /// <summary>
        /// Update an existing driver
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DriverToUpdateDto driverToUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _driverService.UpdateAsync(id, driverToUpdate);
            return NoContent();
        }

        /// <summary>
        /// Delete a driver
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _driverService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Get driver history (routes they've driven)
        /// </summary>
        [HttpGet("{id}/history")]
        public async Task<ActionResult<IEnumerable<object>>> GetDriverHistory(int id)
        {
            var history = await _driverService.GetDriverHistoryAsync(id);
            return Ok(history);
        }

        /// <summary>
        /// Check if driver is available on a specific date
        /// </summary>
        [HttpGet("{id}/availability")]
        public async Task<ActionResult<bool>> CheckAvailability(int id, [FromQuery] DateTime date)
        {
            var isAvailable = await _driverService.IsDriverAvailableAsync(id, date);
            return Ok(new { isAvailable, date });
        }
    }
}
