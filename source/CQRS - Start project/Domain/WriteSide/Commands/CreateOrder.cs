namespace Domain.WriteSide.Commands;

public class CreateOrder : ICommand
{
    public Guid Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
}