namespace Domain.WriteSide;

public interface IHandleCommand<in TCommand> where TCommand : ICommand
{
    void Handle(TCommand command);
}