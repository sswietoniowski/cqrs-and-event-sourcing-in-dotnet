namespace Domain.Events;

public class OrderCreated : IEvent
{
    public Guid Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
}