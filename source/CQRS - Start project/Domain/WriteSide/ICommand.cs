namespace Domain.WriteSide;

public interface ICommand
{
    public Guid Id { get; set; }
}