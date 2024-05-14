using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tributech.SensorManager.Infrastructure.Data;
using Tributech.SensorManager.Application.Data;

namespace Tributech.SensorManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<SensorDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                return;
            }
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<ISensorContext>(sp => sp.GetRequiredService<SensorDbContext>());

        return services;
    }
}