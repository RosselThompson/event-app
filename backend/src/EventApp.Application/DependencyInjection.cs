using EventApp.Application.Events.Commands.CreateEvent;
using Microsoft.Extensions.DependencyInjection;

namespace EventApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<CreateEventHandler>();

        return services;
    }
}
