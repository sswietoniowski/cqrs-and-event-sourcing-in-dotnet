namespace Domain.WriteSide.Commands;

public class DeleteOrderLine : ICommand
{
    public Guid Id { get; set; }
    public Guid OrderLineId { get; set; }
}