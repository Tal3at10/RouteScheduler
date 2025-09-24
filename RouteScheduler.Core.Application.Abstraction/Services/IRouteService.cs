using RouteScheduler.Core.Application.Abstraction.DTOs.Routes;
using RouteScheduler.Core.Application.Abstraction.Common;

namespace RouteScheduler.Core.Application.Abstraction.Services
{
    public interface IRouteService
    {
        Task<IEnumerable<RouteToReturnDto>> GetAllAsync();
        Task<PagedResult<RouteToReturnDto>> GetPagedAsync(PaginationParams pagination, string? origin = null, string? destination = null);
        Task<RouteToReturnDto?> GetByIdAsync(int id);
        Task<RouteToReturnDto> CreateAsync(RouteToCreateDto routeToCreate);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, RouteToUpdateDto routeToUpdate);
        Task<IEnumerable<RouteToReturnDto>> SearchAsync(string? origin = null, string? destination = null);
    }
}
