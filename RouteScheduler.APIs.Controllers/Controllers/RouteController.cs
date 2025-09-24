using Microsoft.AspNetCore.Mvc;
using RouteScheduler.Core.Application.Abstraction.DTOs.Routes;
using RouteScheduler.Core.Application.Abstraction.Services;
using RouteScheduler.Core.Application.Abstraction.Common;

namespace RouteScheduler.APIs.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        /// <summary>
        /// Get all routes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteToReturnDto>>> GetAll()
        {
            var routes = await _routeService.GetAllAsync();
            return Ok(routes);
        }

        /// <summary>
        /// Get routes with pagination and optional filters
        /// </summary>
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<RouteToReturnDto>>> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? origin = null, [FromQuery] string? destination = null)
        {
            var result = await _routeService.GetPagedAsync(new PaginationParams { PageNumber = pageNumber, PageSize = pageSize }, origin, destination);
            return Ok(result);
        }

        /// <summary>
        /// Get route by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RouteToReturnDto>> GetById(int id)
        {
            var route = await _routeService.GetByIdAsync(id);
            if (route == null)
                return NotFound($"Route with ID {id} not found.");
            
            return Ok(route);
        }

        /// <summary>
        /// Create a new route
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RouteToReturnDto>> Create([FromBody] RouteToCreateDto routeToCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var route = await _routeService.CreateAsync(routeToCreate);
            return CreatedAtAction(nameof(GetById), new { id = route.Id }, route);
        }

        /// <summary>
        /// Update an existing route
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RouteToUpdateDto routeToUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _routeService.UpdateAsync(id, routeToUpdate);
            return NoContent();
        }

        /// <summary>
        /// Delete a route
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _routeService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Search routes by origin and/or destination
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<RouteToReturnDto>>> Search([FromQuery] string? origin = null, [FromQuery] string? destination = null)
        {
            var routes = await _routeService.SearchAsync(origin, destination);
            return Ok(routes);
        }
    }
}

