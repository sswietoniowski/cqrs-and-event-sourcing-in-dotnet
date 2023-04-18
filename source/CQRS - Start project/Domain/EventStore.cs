using Domain.Events;

namespace Domain
{
    public class EventStore : IEventStore
    {
        private readonly List<IEvent> _events = new();

        public IEnumerable<IEvent> GetEventsForAggregate(Guid aggregateId) => _events.Where(e => e.Id == aggregateId);

        public void SaveEvents(Guid aggregateId, int eventsLoaded, List<IEvent> events)
        {
            int numberOfExistingEventsForAggregate = _events.Count(e => e.Id == aggregateId);

            if (eventsLoaded != numberOfExistingEventsForAggregate)
            {
                throw new Exception("Concurrency conflict; cannot persist these events.");
            }

            _events.AddRange(events);
        }

        public IEnumerable<IEvent> GetAllEvents() => _events;
    }
}
