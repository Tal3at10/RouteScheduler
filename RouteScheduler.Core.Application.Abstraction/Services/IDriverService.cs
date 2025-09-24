using RouteScheduler.Core.Application.Abstraction.DTOs.Drivers;
using RouteScheduler.Core.Application.Abstraction.DTOs.Routes;
using RouteScheduler.Core.Application.Abstraction.Common;

namespace RouteScheduler.Core.Application.Abstraction.Services
{
    public interface IDriverService
    {
        Task<IEnumerable<DriverToReturnDto>> GetAllAsync();
        Task<PagedResult<DriverToReturnDto>> GetPagedAsync(PaginationParams pagination);
        Task<DriverToReturnDto?> GetByIdAsync(int id);
        Task<IEnumerable<RouteToReturnDto>> GetDriverHistoryAsync(int driverId);
        Task<DriverToReturnDto> CreateAsync(DriverToCreateDto driverToCreate);
        Task DeleteAsync(int id);
        Task<bool> IsDriverAvailableAsync(int driverId, DateTime date);
        Task UpdateAsync(int id, DriverToUpdateDto driverToUpdate);
    }
}
