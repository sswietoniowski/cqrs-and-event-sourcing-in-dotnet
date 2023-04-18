namespace Domain.WriteSide.Commands;

public interface ICommand
{
    public Guid Id { get; set; }
}