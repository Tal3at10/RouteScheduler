using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RouteScheduler.Core.Domain.Contracts;

namespace RouteScheduler.Infrastructure.Persistence.Data
{
    public class ContextIntializer : IContextIntializer
    {
        private readonly Context _dbcontext;
        public ContextIntializer(Context dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task InitializeAsync()
        {
            var pendingMigrations = _dbcontext.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
                await _dbcontext.Database.MigrateAsync(); //Update the database to the latest migration

        }
    }
}
