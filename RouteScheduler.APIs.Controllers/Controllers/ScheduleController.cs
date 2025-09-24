using Microsoft.AspNetCore.Mvc;
using RouteScheduler.Core.Application.Abstraction.DTOs.Scheduals;
using RouteScheduler.Core.Application.Abstraction.Services;

namespace RouteScheduler.APIs.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        /// <summary>
        /// Get all schedules
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleToReturnDto>>> GetAll()
        {
            var schedules = await _scheduleService.GetAllAsync();
            return Ok(schedules);
        }

        /// <summary>
        /// Get schedule by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleToReturnDto>> GetById(int id)
        {
            var schedule = await _scheduleService.GetByIdAsync(id);
            if (schedule == null)
                return NotFound($"Schedule with ID {id} not found.");
            
            return Ok(schedule);
        }

        /// <summary>
        /// Create a new schedule
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ScheduleToReturnDto>> Create([FromBody] ScheduleToCreateDto scheduleToCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var schedule = await _scheduleService.CreateAsync(scheduleToCreate);
            return CreatedAtAction(nameof(GetById), new { id = schedule.Id }, schedule);
        }

        /// <summary>
        /// Update an existing schedule
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ScheduleToUpdateDto scheduleToUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _scheduleService.UpdateAsync(id, scheduleToUpdate);
            return NoContent();
        }

        /// <summary>
        /// Delete a schedule
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _scheduleService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Get schedules by driver ID
        /// </summary>
        [HttpGet("driver/{driverId}")]
        public async Task<ActionResult<IEnumerable<ScheduleToReturnDto>>> GetByDriver(int driverId)
        {
            var schedules = await _scheduleService.GetByDriverAsync(driverId);
            return Ok(schedules);
        }

        /// <summary>
        /// Get schedules by date
        /// </summary>
        [HttpGet("date/{date:datetime}")]
        public async Task<ActionResult<IEnumerable<ScheduleToReturnDto>>> GetByDate(DateTime date)
        {
            var schedules = await _scheduleService.GetByDateAsync(date);
            return Ok(schedules);
        }

        /// <summary>
        /// Get schedules by status
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<ScheduleToReturnDto>>> GetByStatus(string status)
        {
            var schedules = await _scheduleService.GetByStatusAsync(status);
            return Ok(schedules);
        }

        /// <summary>
        /// Check if driver is available on a specific date
        /// </summary>
        [HttpGet("availability/{driverId}")]
        public async Task<ActionResult<bool>> CheckDriverAvailability(int driverId, [FromQuery] DateTime date)
        {
            var isAvailable = await _scheduleService.IsDriverAvailableAsync(driverId, date);
            return Ok(new { isAvailable, driverId, date });
        }
    }
}

