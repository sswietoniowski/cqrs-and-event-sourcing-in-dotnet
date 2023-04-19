namespace Domain.Events
{
    public interface IReadEventStore
    {
        void ForAllEvents(Action<IEvent> apply);
    }
}
