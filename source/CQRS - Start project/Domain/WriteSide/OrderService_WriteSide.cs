namespace Domain.WriteSide;

public class OrderServiceWriteSide :
    IHandleCommand<CreateOrder>,
    IHandleCommand<AddOrderLine>,
    IHandleCommand<CancelOrder>,
    IHandleCommand<DeleteOrderLine>
{
    private readonly IOrderRepository _repository;

    public OrderServiceWriteSide(IOrderRepository repository)
    {
        this._repository = repository;
    }

    public void Handle(AddOrderLine command)
    {
        var order = _repository.Load(command.Id);

        var orderLineId = Guid.NewGuid();
        command.OrderLine.Id = orderLineId;
        order.OrderLines.Add(command.OrderLine);

        _repository.Update(command.Id, order);
    }

    public void Handle(DeleteOrderLine command)
    {
        var order = _repository.Load(command.Id);

        var ol = order.OrderLines.FirstOrDefault(ol => ol.Id == command.OrderLineId);
        if (ol != null)
            order.OrderLines.Remove(ol);

        _repository.Update(command.Id, order);
    }

    public void Handle(CancelOrder command)
    {
        var order = _repository.Load(command.Id);

        order.OrderState = OrderState.Cancel;

        _repository.Update(command.Id, order);
    }

    public void Handle(CreateOrder command)
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