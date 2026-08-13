using EventApp.Application.Events.Commands.CreateEvent;
using EventApp.Application.Events.Queries.GetEventById;
using Microsoft.Extensions.DependencyInjection;

namespace EventApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<CreateEventHandler>();
        services.AddScoped<GetEventByIdHandler>();

        return services;
    }
}
