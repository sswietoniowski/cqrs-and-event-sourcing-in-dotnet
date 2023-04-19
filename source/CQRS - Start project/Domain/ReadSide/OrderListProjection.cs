using Domain.Events;

namespace Domain.ReadSide;

public class OrderListProjection : Projection,
    IBuildFrom<OrderCreated>,
    IBuildFrom<OrderCancelled>,
    IBuildFrom<OrderLineAdded>,
    IBuildFrom<OrderLineDeleted>

{
    private readonly Dictionary<Guid, OrderDetails> _orderDict = new();

    private class OrderDetails
    {
        public Guid Id { get; set; }
        public OrderState orderState { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal OrderValue { get; set; }
        public List<OrderLine> orderLines { get; set; }
    }

    public List<OrderSummary> GetOrderSummaryList()
    {
        return _orderDict.Values.Select(o =>
            new OrderSummary()
            {
                Id = o.Id,
                OrderState = o.orderState,
                CustomerId = o.CustomerId,
                CustomerName = o.CustomerName,
                OrderValue = o.OrderValue,
            }
        ).ToList();
    }

    public void Apply(OrderCreated e)
    {
        _orderDict.Add(e.Id, new OrderDetails()
        {
            Id = e.Id,
            CustomerId = e.CustomerId,
            CustomerName = e.CustomerName,
            orderState = OrderState.New,
            OrderValue = 0,
            orderLines = new List<OrderLine>()
        });
    }

    public void Apply(OrderCancelled e)
    {
        _orderDict[e.Id].orderState = OrderState.Cancel;
    }

    public void Apply(OrderLineAdded e)
    {
        _orderDict[e.Id].orderLines.Add(e.OrderLine);
        _orderDict[e.Id].OrderValue = CalculateOrderValue(_orderDict[e.Id]);
    }

    public void Apply(OrderLineDeleted e)
    {
        _orderDict[e.Id].orderLines.RemoveAll(ol => ol.Id == e.Id);
        _orderDict[e.Id].OrderValue = CalculateOrderValue(_orderDict[e.Id]);
    }

    private decimal CalculateOrderValue(OrderDetails order)
    {
        decimal totalValue = 0;
        foreach (var line in order.orderLines)
        {
            totalValue = totalValue + line.Price * line.Quantity;
        }

        return totalValue;
    }
}