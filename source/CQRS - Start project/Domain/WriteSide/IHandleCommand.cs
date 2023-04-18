using Domain.Events;
using Domain.WriteSide.Commands;

namespace Domain.WriteSide;

public interface IHandleCommand<in TCommand> where TCommand : ICommand
{
    IEnumerable<IEvent> Handle(TCommand command);
}