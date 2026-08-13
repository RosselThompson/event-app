using EventApp.Infrastructure.Events.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<EventsDbContext>(options => options.UseNpgsql(connectionString));
        services
        .AddHealthChecks()
        .AddDbContextCheck<EventsDbContext>(name: "events-db");
        return services;
    }
}


