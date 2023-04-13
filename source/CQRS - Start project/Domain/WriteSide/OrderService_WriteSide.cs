namespace Domain.WriteSide;

public class OrderService_WriteSide : IOrderService_WriteSide, IHandleCommand<AddOrderLine>, IHandleCommand<DeleteOrderLine>,
    IHandleCommand<CancelOrder>, IHandleCommand<CreateOrder>
{
    private readonly IOrderRepository repository;

    public OrderService_WriteSide(IOrderRepository repository)
    {
        this.repository = repository;
    }

    public void Handle(AddOrderLine command)
    {
        var order = repository.Load(command.Id);
        var orderLineId = Guid.NewGuid();
        command.OrderLine.Id = orderLineId;
        order.orderLines.Add(command.OrderLine);
    }

    public void CreateOrder(Guid orderId, int customerId, string customerName)
    {
        var order = new Order()
        {
            Id = orderId,
            CustomerId = customerId,
            CustomerName = customerName,
            orderState = OrderState.New,
            orderLines = new List<OrderLine>()
        };

        repository.Insert(orderId, order);
    }

    public void Handle(DeleteOrderLine command)
    {
        var order = repository.Load(command.Id);

        var ol = order.orderLines.FirstOrDefault(ol => ol.Id == command.OrderLineId);
        if (ol != null)
            order.orderLines.Remove(ol);

        repository.Update(command.Id, order);
    }

    public void Handle(CancelOrder command)
    {
        var order = repository.Load(command.Id);

        order.orderState = OrderState.cancel;

        repository.Update(command.Id, order);
    }

    public void Handle(CreateOrder command)
    {
        var order = new Order()
        {
            Id = command.Id,
            CustomerId = command.CustomerId,
            CustomerName = command.CustomerName,
            orderState = OrderState.New,
            orderLines = new List<OrderLine>()
        };

        repository.Insert(command.Id, order);
    }

    private decimal CalculateOrderValue(Order order)
    {
        decimal totalValue = 0;
        foreach (var line in order.orderLines)
        {
            totalValue = totalValue + line.Price * line.Quantity;
        }

        return totalValue;
    }
}