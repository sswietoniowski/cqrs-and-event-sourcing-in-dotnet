namespace Domain.Events;

public class EventStore : IEventStore, IReadEventStore
{
    private readonly List<IEvent> _events = new();
    private Action<IEvent> _eventCallback;
    private int _eventRead = 0;

    public IEnumerable<IEvent> LoadEvents(Guid aggregateId) => _events.Where(e => e.Id == aggregateId);

    public void SaveEvents(Guid aggregateId, int eventsLoaded, List<IEvent> events)
    {
        int numberOfExistingEventsForAggregate = _events.Count(e => e.Id == aggregateId);

        if (eventsLoaded != numberOfExistingEventsForAggregate)
        {
            throw new Exception("Concurrency conflict; cannot persist these events.");
        }

        _events.AddRange(events);

        PublishNewEvents();
    }

    public IEnumerable<IEvent> GetAllEvents() => _events;

    public void ForAllEvents(Action<IEvent> apply)
    {
        _eventCallback = apply;

        PublishNewEvents();
    }

    private void PublishNewEvents()
    {
        if (_eventCallback != null)
        {
            while ((_events.Count()) > _eventRead)
            {
                var ev = _events[_eventRead];
                _eventCallback(ev);
                _eventRead++;
            }
        }
    }
}