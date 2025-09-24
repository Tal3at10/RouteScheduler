using AutoMapper;
using RouteScheduler.Core.Application.Abstraction.DTOs.Scheduals;
using RouteScheduler.Core.Application.Abstraction.Services;
using RouteScheduler.Core.Domain.Contracts;
using RouteScheduler.Core.Domain.Entities;

namespace RouteScheduler.Core.Application.Services.Schedules
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ScheduleToReturnDto>> GetAllAsync()
        {
            var schedules = await _unitOfWork.Schedules.GetAllAsync();
            return _mapper.Map<IEnumerable<ScheduleToReturnDto>>(schedules);
        }

        public async Task<ScheduleToReturnDto?> GetByIdAsync(int id)
        {
            var schedule = await _unitOfWork.Schedules.GetByIdAsync(id);
            return schedule == null ? null : _mapper.Map<ScheduleToReturnDto>(schedule);
        }

        public async Task<ScheduleToReturnDto> CreateAsync(ScheduleToCreateDto scheduleToCreate)
        {
            // Check if driver is available
            var isAvailable = await _unitOfWork.Schedules.ExistsAsync(s => 
                s.DriverId == scheduleToCreate.DriverId && 
                s.Date.Date == scheduleToCreate.Date.Date && 
                s.Status != "Cancelled");

            if (isAvailable)
            {
                throw new InvalidOperationException("Driver is not available on the specified date.");
            }

            var schedule = _mapper.Map<Schedule>(scheduleToCreate);
            await _unitOfWork.Schedules.AddAsync(schedule);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ScheduleToReturnDto>(schedule);
        }

        public async Task DeleteAsync(int id)
        {
            var schedule = await _unitOfWork.Schedules.GetByIdAsync(id);
            if (schedule != null)
            {
                _unitOfWork.Schedules.Remove(schedule);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(int id, ScheduleToUpdateDto scheduleToUpdate)
        {
            var schedule = await _unitOfWork.Schedules.GetByIdAsync(id);
            if (schedule != null)
            {
                _mapper.Map(scheduleToUpdate, schedule);
                _unitOfWork.Schedules.Update(schedule);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ScheduleToReturnDto>> GetByDriverAsync(int driverId)
        {
            var schedules = await _unitOfWork.Schedules.FindAsync(s => s.DriverId == driverId);
            return _mapper.Map<IEnumerable<ScheduleToReturnDto>>(schedules);
        }

        public async Task<IEnumerable<ScheduleToReturnDto>> GetByDateAsync(DateTime date)
        {
            var schedules = await _unitOfWork.Schedules.FindAsync(s => s.Date.Date == date.Date);
            return _mapper.Map<IEnumerable<ScheduleToReturnDto>>(schedules);
        }

        public async Task<IEnumerable<ScheduleToReturnDto>> GetByStatusAsync(string status)
        {
            var schedules = await _unitOfWork.Schedules.FindAsync(s => s.Status == status);
            return _mapper.Map<IEnumerable<ScheduleToReturnDto>>(schedules);
        }

        public async Task<bool> IsDriverAvailableAsync(int driverId, DateTime date)
        {
            return !await _unitOfWork.Schedules.ExistsAsync(s => 
                s.DriverId == driverId && 
                s.Date.Date == date.Date && 
                s.Status != "Cancelled");
        }
    }
}
