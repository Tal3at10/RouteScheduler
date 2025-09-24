using AutoMapper;
using RouteScheduler.Core.Application.Abstraction.DTOs.Drivers;
using RouteScheduler.Core.Application.Abstraction.DTOs.Routes;
using RouteScheduler.Core.Application.Abstraction.DTOs.Scheduals;
using RouteScheduler.Core.Domain.Entities;

namespace RouteScheduler.Core.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Driver mappings
            CreateMap<Driver, DriverToReturnDto>();
            CreateMap<DriverToCreateDto, Driver>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
            CreateMap<DriverToUpdateDto, Driver>()
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Route mappings
            CreateMap<Route, RouteToReturnDto>();
            CreateMap<RouteToCreateDto, Route>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
            CreateMap<RouteToUpdateDto, Route>()
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Schedule mappings
            CreateMap<Schedule, ScheduleToReturnDto>()
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver.Name))
                .ForMember(dest => dest.RouteOrigin, opt => opt.MapFrom(src => src.Route.Origin))
                .ForMember(dest => dest.RouteDestination, opt => opt.MapFrom(src => src.Route.Destination));
            
            CreateMap<ScheduleToCreateDto, Schedule>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"));
            
            CreateMap<ScheduleToUpdateDto, Schedule>()
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
