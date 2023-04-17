namespace Domain;

public class OrderServiceReadSide : IOrderServiceReadSide
{
    private readonly IOrderRepository _repository;

    public OrderServiceReadSide(IOrderRepository repository)
    {
        this._repository = repository;
    }

    public Order LoadOrder(Guid orderId)
    {
        var order = _repository.Load(orderId);

        order.OrderValue = CalculateOrderValue(order);

        return order;
    }

    public List<OrderSummary> LoadAllOrders()
    {
        var orders = _repository.LoadAllOrders();

        var orderSummaryList = orders.Select(o => new OrderSummary()
        {
            Id = o.Id,
            CustomerId = o.CustomerId,
            CustomerName = o.CustomerName,
            OrderState = o.OrderState,
            OrderValue = CalculateOrderValue(o)
        }
        ).ToList();

        return orderSummaryList;

    }

    private decimal CalculateOrderValue(Order order)
    {
        decimal totalValue = 0;
        foreach (var line in order.OrderLines)
        {
            totalValue += line.Price * line.Quantity;
        }

        return totalValue;
    }
}