using Domain.Events;

namespace Domain.WriteSide;

public abstract class Aggregate
{
    public void ApplyEvents(IEnumerable<IEvent> events)
    {
        foreach (var e in events)
        {
            var eventType = e.GetType();

            var method = this.GetType().GetMethod("Apply", new[] { eventType });

            if (method is null)
            {
                throw new Exception($"Aggregate {this.GetType().Name} does not have an Apply method for event {eventType.Name}.");
            }

            method.Invoke(this, new object[] { e });
        }
    }
}