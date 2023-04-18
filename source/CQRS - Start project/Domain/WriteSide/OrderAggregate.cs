using Domain.Events;
using Domain.WriteSide.Commands;

namespace Domain.WriteSide;

public class OrderAggregate : Aggregate,
    IHandleCommand<CreateOrder>,
    IHandleCommand<AddOrderLine>,
    IHandleCommand<CancelOrder>,
    IHandleCommand<DeleteOrderLine>
{
    private readonly IOrderRepository _repository;

    public OrderAggregate(IOrderRepository repository)
    {
        this._repository = repository;
    }

    public IEnumerable<IEvent> Handle(AddOrderLine command)
    {
        var order = _repository.Load(command.Id);

        var orderLineId = Guid.NewGuid();
        command.OrderLine.Id = orderLineId;
        order.OrderLines.Add(command.OrderLine);

        _repository.Update(command.Id, order);

        yield return new OrderLineAdded()
        {
            Id = command.Id,
            OrderLine = command.OrderLine
        };
    }

    public IEnumerable<IEvent> Handle(DeleteOrderLine command)
    {
        var order = _repository.Load(command.Id);

        var ol = order.OrderLines.FirstOrDefault(ol => ol.Id == command.OrderLineId);
        if (ol != null)
            order.OrderLines.Remove(ol);

        _repository.Update(command.Id, order);

        yield return new OrderLineDeleted()
        {
            Id = command.Id,
            OrderLineId = command.OrderLineId
        };
    }

    public IEnumerable<IEvent> Handle(CancelOrder command)
    {
        var order = _repository.Load(command.Id);

        order.OrderState = OrderState.Cancel;

        _repository.Update(command.Id, order);

        yield return new OrderCanceled()
        {
            Id = command.Id
        };
    }

    public IEnumerable<IEvent> Handle(CreateOrder command)
    {
        var order = new Order()
        {
            Id = command.Id,
            CustomerId = command.CustomerId,
            CustomerName = command.CustomerName,
            OrderState = OrderState.New,
            OrderLines = new List<OrderLine>()
        };

        _repository.Insert(command.Id, order);

        yield return new OrderCreated()
        {
            Id = command.Id,
            CustomerId = command.CustomerId,
            CustomerName = command.CustomerName
        };
    }

    private decimal CalculateOrderValue(Order order)
    {
        decimal totalValue = 0;
        foreach (var line in order.OrderLines)
        {
            totalValue = totalValue + line.Price * line.Quantity;
        }

        return totalValue;
    }
}