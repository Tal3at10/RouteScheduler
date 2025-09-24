using RouteScheduler.Core.Domain.Entities;

namespace RouteScheduler.Core.Domain.Contracts
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IGenericRepository<Driver, int> Drivers { get; }
        IGenericRepository<Route, int> Routes { get; }
        IGenericRepository<Schedule, int> Schedules { get; }
        Task<int> SaveChangesAsync();
    }
}
