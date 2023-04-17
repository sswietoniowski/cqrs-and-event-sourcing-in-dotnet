namespace Domain.WriteSide;

public interface IWriteService
{
    void HandleCommand<TCommand>(TCommand command) where TCommand : ICommand;
}