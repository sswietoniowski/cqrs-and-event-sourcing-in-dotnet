namespace Domain.Events;

public interface IEventStore
{
    IEnumerable<IEvent> LoadEvents(Guid aggregateId);
    void SaveEvents(Guid aggregateId, int eventsLoaded, List<IEvent> events);
    IEnumerable<IEvent> GetAllEvents();
}