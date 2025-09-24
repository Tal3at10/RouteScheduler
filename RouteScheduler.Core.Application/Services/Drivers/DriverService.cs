using AutoMapper;
using RouteScheduler.Core.Application.Abstraction.DTOs.Drivers;
using RouteScheduler.Core.Application.Abstraction.DTOs.Routes;
using RouteScheduler.Core.Application.Abstraction.Common;
using RouteScheduler.Core.Application.Abstraction.Services;
using RouteScheduler.Core.Domain.Contracts;
using RouteScheduler.Core.Domain.Entities;

namespace RouteScheduler.Core.Application.Services.Drivers
{
    public class DriverService : IDriverService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DriverService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DriverToReturnDto>> GetAllAsync()
        {
            var drivers = await _unitOfWork.Drivers.GetAllAsync();
            return _mapper.Map<IEnumerable<DriverToReturnDto>>(drivers);
        }

        public async Task<PagedResult<DriverToReturnDto>> GetPagedAsync(PaginationParams pagination)
        {
            var (items, total) = await _unitOfWork.Drivers.GetPagedAsync(
                pagination.PageNumber,
                pagination.PageSize,
                null,
                q => q.OrderBy(d => d.Id)
            );

            return new PagedResult<DriverToReturnDto>
            {
                Items = _mapper.Map<IReadOnlyList<DriverToReturnDto>>(items),
                TotalCount = total,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<DriverToReturnDto?> GetByIdAsync(int id)
        {
            var driver = await _unitOfWork.Drivers.GetByIdAsync(id);
            return driver == null ? null : _mapper.Map<DriverToReturnDto>(driver);
        }

        public async Task<IEnumerable<RouteToReturnDto>> GetDriverHistoryAsync(int driverId)
        {
            var schedules = await _unitOfWork.Schedules.FindAsync(s => s.DriverId == driverId);
            var routes = schedules.Select(s => s.Route).Distinct();
            return _mapper.Map<IEnumerable<RouteToReturnDto>>(routes);
        }

        public async Task<DriverToReturnDto> CreateAsync(DriverToCreateDto driverToCreate)
        {
            var driver = _mapper.Map<Driver>(driverToCreate);
            await _unitOfWork.Drivers.AddAsync(driver);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<DriverToReturnDto>(driver);
        }

        public async Task DeleteAsync(int id)
        {
            var driver = await _unitOfWork.Drivers.GetByIdAsync(id);
            if (driver != null)
            {
                _unitOfWork.Drivers.Remove(driver);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> IsDriverAvailableAsync(int driverId, DateTime date)
        {
            var hasSchedule = await _unitOfWork.Schedules.ExistsAsync(s => 
                s.DriverId == driverId && 
                s.Date.Date == date.Date && 
                s.Status != "Cancelled");
            
            return !hasSchedule;
        }

        public async Task UpdateAsync(int id, DriverToUpdateDto driverToUpdate)
        {
            var driver = await _unitOfWork.Drivers.GetByIdAsync(id);
            if (driver != null)
            {
                _mapper.Map(driverToUpdate, driver);
                _unitOfWork.Drivers.Update(driver);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
