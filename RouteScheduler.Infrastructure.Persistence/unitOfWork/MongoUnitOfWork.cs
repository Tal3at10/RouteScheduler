using RouteScheduler.Core.Domain.Contracts;
using RouteScheduler.Core.Domain.Entities;
using RouteScheduler.Infrastructure.Persistence.Data.Mongo;
using RouteScheduler.Infrastructure.Persistence.Repositories.Mongo;

namespace RouteScheduler.Infrastructure.Persistence.unitOfWork
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        private readonly MongoContext _context;

        private IGenericRepository<Driver, int>? _driverRepository;
        private IGenericRepository<Route, int>? _routeRepository;
        private IGenericRepository<Schedule, int>? _scheduleRepository;

        public MongoUnitOfWork(MongoContext context)
        {
            _context = context;
        }

        public IGenericRepository<Driver, int> Drivers =>
            _driverRepository ??= new MongoGenericRepository<Driver, int>(_context);

        public IGenericRepository<Route, int> Routes =>
            _routeRepository ??= new MongoGenericRepository<Route, int>(_context);

        public IGenericRepository<Schedule, int> Schedules =>
            _scheduleRepository ??= new MongoGenericRepository<Schedule, int>(_context);

        public Task<int> SaveChangesAsync()
        {
            // Mongo operations are immediate; return 0 changes notionally
            return Task.FromResult(0);
        }

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}




