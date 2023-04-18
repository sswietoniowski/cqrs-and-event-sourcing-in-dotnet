namespace Domain.Events;

public class OrderCanceled : IEvent
{
    public Guid Id { get; set; }
}