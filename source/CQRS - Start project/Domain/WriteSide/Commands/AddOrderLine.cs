namespace Domain.WriteSide.Commands;

public class AddOrderLine : ICommand
{
    public Guid Id { get; set; }
    public OrderLine OrderLine { get; set; }
}