using Domain.Events;
using Domain.WriteSide.Commands;

namespace Domain.WriteSide;

public class OrderAggregate : Aggregate,
    IHandleCommand<CreateOrder>,
    IHandleCommand<AddOrderLine>,
    IHandleCommand<CancelOrder>,
    IHandleCommand<DeleteOrderLine>,
    IApplyEvent<OrderCreated>,
    IApplyEvent<OrderLineAdded>,
    IApplyEvent<OrderCanceled>,
    IApplyEvent<OrderLineDeleted>
{
    private Guid _orderId;
    private int _customerId;
    private string _customerName;
    private OrderState _orderState;
    private List<OrderLine> _orderLines = new();

    public OrderAggregate()
    {
    }

    public IEnumerable<IEvent> Handle(AddOrderLine command)
    {
        // TODO: Add validation

        if (_orderState == OrderState.Cancel)
        {
            throw new Exception("Can't modify cancelled order.");
        }

        yield return new OrderLineAdded()
        {
            Id = command.Id,
            OrderLine = command.OrderLine
        };
    }

    public IEnumerable<IEvent> Handle(DeleteOrderLine command)
    {
        // TODO: Add validation

        if (_orderState == OrderState.Cancel)
        {
            throw new Exception("Can't modify cancelled order.");
        }

        yield return new OrderLineDeleted()
        {
            Id = command.Id,
            OrderLineId = command.OrderLineId
        };
    }

    public IEnumerable<IEvent> Handle(CancelOrder command)
    {
        // TODO: Add validation

        yield return new OrderCanceled()
        {
            Id = command.Id
        };
    }

    public IEnumerable<IEvent> Handle(CreateOrder command)
    {
        // TODO: Add validation

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

    public void Apply(OrderCreated e)
    {
        _orderId = e.Id;
        _customerId = e.CustomerId;
        _customerName = e.CustomerName;
        _orderState = OrderState.New;
    }

    public void Apply(OrderLineAdded e)
    {
        _orderLines.Add(e.OrderLine);
    }

    public void Apply(OrderCanceled e)
    {
        _orderState = OrderState.Cancel;
    }

    public void Apply(OrderLineDeleted e)
    {
        _orderLines.RemoveAll(ol => ol.Id == e.OrderLineId);
    }
}