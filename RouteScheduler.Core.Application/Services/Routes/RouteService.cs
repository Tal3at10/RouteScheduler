using AutoMapper;
using RouteScheduler.Core.Application.Abstraction.Common;
using RouteScheduler.Core.Application.Abstraction.DTOs.Routes;
using RouteScheduler.Core.Application.Abstraction.Services;
using RouteScheduler.Core.Domain.Contracts;
using RouteScheduler.Core.Domain.Entities;

namespace RouteScheduler.Core.Application.Services.Routes
{
    public class RouteService : IRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RouteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RouteToReturnDto>> GetAllAsync()
        {
            var routes = await _unitOfWork.Routes.GetAllAsync();
            return _mapper.Map<IEnumerable<RouteToReturnDto>>(routes);
        }

        public async Task<RouteToReturnDto?> GetByIdAsync(int id)
        {
            var route = await _unitOfWork.Routes.GetByIdAsync(id);
            return route == null ? null : _mapper.Map<RouteToReturnDto>(route);
        }

        public async Task<RouteToReturnDto> CreateAsync(RouteToCreateDto routeToCreate)
        {
            var route = _mapper.Map<Route>(routeToCreate);
            await _unitOfWork.Routes.AddAsync(route);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<RouteToReturnDto>(route);
        }

        public async Task DeleteAsync(int id)
        {
            var route = await _unitOfWork.Routes.GetByIdAsync(id);
            if (route != null)
            {
                _unitOfWork.Routes.Remove(route);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(int id, RouteToUpdateDto routeToUpdate)
        {
            var route = await _unitOfWork.Routes.GetByIdAsync(id);
            if (route != null)
            {
                _mapper.Map(routeToUpdate, route);
                _unitOfWork.Routes.Update(route);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<RouteToReturnDto>> SearchAsync(string? origin = null, string? destination = null)
        {
            var routes = await _unitOfWork.Routes.FindAsync(r =>
                (origin == null || r.Origin.Contains(origin)) &&
                (destination == null || r.Destination.Contains(destination))
            );

            return _mapper.Map<IEnumerable<RouteToReturnDto>>(routes);
        }

        public async Task<PagedResult<RouteToReturnDto>> GetPagedAsync(PaginationParams pagination, string? origin = null, string? destination = null)
        {
            var (items, total) = await _unitOfWork.Routes.GetPagedAsync(
                pagination.PageNumber,
                pagination.PageSize,
                r =>
                    (origin == null || r.Origin.Contains(origin)) &&
                    (destination == null || r.Destination.Contains(destination)),
                q => q.OrderBy(r => r.Id)
            );

            return new PagedResult<RouteToReturnDto>
            {
                Items = _mapper.Map<IReadOnlyList<RouteToReturnDto>>(items),
                TotalCount = total,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
    }
}
