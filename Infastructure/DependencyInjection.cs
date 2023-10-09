using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(IServiceCollection services)
        {
            // Other services and configurations...

            // Register your DbContext (Entity Framework Core)
            services.AddDbContext<UsersDbContext>(options => options.UseSqlServer("Data Source=DESKTOP-O7KPC84\\MSSQLSERVER01;" +
  "Initial Catalog=Users;"
));

            // Register the database connection as a service
            return services;
        }
    }
}
