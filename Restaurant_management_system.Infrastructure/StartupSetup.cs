using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurant_management_system.Infrastructure.Data;

namespace Restaurant_management_system.Infrastructure
{
    public static class StartupSetup
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString)
            );
    }
}

