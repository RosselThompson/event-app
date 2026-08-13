using EventApp.Domain.Events.Entities;

namespace EventApp.Application.Events;

public interface IEventRepository
{
    void Add(Event eventItem);
    // void Update(Event eventItem);
    // void Delete(Event eventItem);
    // Task<Event> GetByIdAsync(Guid id);
    // Task<List<Event>> GetAllAsync();
}
