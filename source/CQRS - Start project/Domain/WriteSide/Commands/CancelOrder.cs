namespace Domain.WriteSide.Commands;

public class CancelOrder : ICommand
{
    public Guid Id { get; set; }
}