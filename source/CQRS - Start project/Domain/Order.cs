namespace Domain;

public class Order
{
    public Guid Id { get; set; }
    public OrderState OrderState { get; set; } = OrderState.Unknown;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public decimal OrderValue { get; set; }
    public List<OrderLine> OrderLines { get; set; } = new();
}