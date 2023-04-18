namespace Domain.Events;

public class OrderLineDeleted : IEvent
{
    public Guid Id { get; set; }
    public Guid OrderLineId { get; set; }
}