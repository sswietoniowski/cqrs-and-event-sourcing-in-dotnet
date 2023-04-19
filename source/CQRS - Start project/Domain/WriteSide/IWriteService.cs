using System.ComponentModel;
using Domain.WriteSide.Commands;

namespace Domain.WriteSide;

public interface IWriteService
{
    void HandleCommand<TCommand>(TCommand command) where TCommand : ICommand;

    TResult QueryAggregate<TAggregate, TResult>(Guid id, Func<TAggregate, TResult> query) where TAggregate : Aggregate, new();
}