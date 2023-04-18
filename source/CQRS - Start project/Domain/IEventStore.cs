using Domain.Events;

namespace Domain
{
    public interface IEventStore
    {
        IEnumerable<IEvent> GetEventsForAggregate(Guid aggregateId);
        void SaveEvents(Guid aggregateId, int eventsLoaded, List<IEvent> events);
        IEnumerable<IEvent> GetAllEvents();
    }
}
