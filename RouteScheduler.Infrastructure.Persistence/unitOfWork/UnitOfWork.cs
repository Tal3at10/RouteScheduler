using Microsoft.EntityFrameworkCore;
using RouteScheduler.Core.Domain.Contracts;
using RouteScheduler.Core.Domain.Entities;
using RouteScheduler.Infrastructure.Persistence.Data;
using RouteScheduler.Infrastructure.Persistence.Repositories;

namespace RouteScheduler.Infrastructure.Persistence.unitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;
        private IGenericRepository<Driver, int>? _driverRepository;
        private IGenericRepository<Route, int>? _routeRepository;
        private IGenericRepository<Schedule, int>? _scheduleRepository;

        public UnitOfWork(Context context)
        {
            _context = context;
        }

        public IGenericRepository<Driver, int> Drivers => 
            _driverRepository ??= new GenericRepository<Driver, int>(_context);

        public IGenericRepository<Route, int> Routes => 
            _routeRepository ??= new GenericRepository<Route, int>(_context);

        public IGenericRepository<Schedule, int> Schedules => 
            _scheduleRepository ??= new GenericRepository<Schedule, int>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
