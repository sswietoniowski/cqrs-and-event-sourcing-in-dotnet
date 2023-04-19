namespace Domain.Events;

public class OrderCancelled : IEvent
{
    public Guid Id { get; set; }
}