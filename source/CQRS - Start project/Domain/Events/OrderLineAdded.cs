namespace Domain.Events;

public class OrderLineAdded : IEvent
{
    public Guid Id { get; set; }
    public OrderLine OrderLine { get; set; }
}