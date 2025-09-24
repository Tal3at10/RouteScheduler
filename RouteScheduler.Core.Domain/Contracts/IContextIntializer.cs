using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteScheduler.Core.Domain.Contracts
{
    public interface IContextIntializer
    {
        Task InitializeAsync();
        //Task SeedAsync();
    }
}
