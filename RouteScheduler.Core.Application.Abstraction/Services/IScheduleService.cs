using RouteScheduler.Core.Application.Abstraction.DTOs.Scheduals;

namespace RouteScheduler.Core.Application.Abstraction.Services
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleToReturnDto>> GetAllAsync();
        Task<ScheduleToReturnDto?> GetByIdAsync(int id);
        Task<ScheduleToReturnDto> CreateAsync(ScheduleToCreateDto scheduleToCreate);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, ScheduleToUpdateDto scheduleToUpdate);
        Task<IEnumerable<ScheduleToReturnDto>> GetByDriverAsync(int driverId);
        Task<IEnumerable<ScheduleToReturnDto>> GetByDateAsync(DateTime date);
        Task<IEnumerable<ScheduleToReturnDto>> GetByStatusAsync(string status);
        Task<bool> IsDriverAvailableAsync(int driverId, DateTime date);
    }
}
