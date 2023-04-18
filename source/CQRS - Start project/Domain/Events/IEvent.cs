namespace Domain.Events;

public interface IEvent
{
    public Guid Id { get; set; }
}