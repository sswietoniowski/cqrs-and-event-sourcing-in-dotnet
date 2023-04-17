namespace Domain;


public interface IOrderRepository
{
    // Commands
    void Insert(Guid orderId, Order order);
    void Update(Guid orderId, Order order);

    // Queries
    Order Load(Guid orderId);
    List<Order> LoadAllOrders();
}

public class OrderRepository : IOrderRepository
{
    private Dictionary<Guid, Order> _ordersDb = new();

    public OrderRepository()
    {
        var order1 = new Order()
        {
            Id = Guid.NewGuid(),
            CustomerId = 1001,
            CustomerName = "Bob",
            OrderState = OrderState.New,
            OrderLines = new()
            {
                new OrderLine()
                {
                    Id = Guid.NewGuid(),
                    Name = "Widget A",
                    ProductId = 101,
                    Price = 5.95m,
                    Quantity = 10
                },
                new OrderLine()
                {
                    Id = Guid.NewGuid(),
                    Name = "MegaMax B",
                    ProductId = 102,
                    Price = 24.95m,
                    Quantity = 1
                }
            }
        };

        var order2 = new Order()
        {
            Id = Guid.NewGuid(),
            CustomerId = 1002,
            CustomerName = "Alice",
            OrderState = OrderState.Paid,
            OrderLines = new()
            {
                new OrderLine()
                {
                    Id = Guid.NewGuid(),
                    Name = "SuperMax",
                    ProductId = 103,
                    Price = 85.95m,
                    Quantity = 2
                },
                new OrderLine()
                {
                    Id = Guid.NewGuid(),
                    Name = "MiniMax",
                    ProductId = 104,
                    Price = 19.95m,
                    Quantity = 3
                }
            }
        };

        var order3 = new Order()
        {
            Id = Guid.NewGuid(),
            CustomerId = 1003,
            CustomerName = "Joe",
            OrderState = OrderState.Cancel,
            OrderLines = new()
            {
                new OrderLine()
                {
                    Id = Guid.NewGuid(),
                    Name = "SuperMax",
                    ProductId = 103,
                    Price = 85.95m,
                    Quantity = 2
                },
                new OrderLine()
                {
                    Id = Guid.NewGuid(),
                    Name = "MiniMax",
                    ProductId = 106,
                    Price = 29.95m,
                    Quantity = 1
                }
            }
        };

        _ordersDb.Add(order1.Id, order1);
        _ordersDb.Add(order2.Id, order2);
        _ordersDb.Add(order3.Id, order3);
    }

    public void Insert(Guid orderId, Order order)
    {
        order.Id = orderId;

        _ordersDb.Add(orderId, order);
    }

    public Order Load(Guid orderId)
    {
        return _ordersDb[orderId];
    }

    public List<Order> LoadAllOrders()
    {
        return _ordersDb.Values.ToList();
    }

    public void Update(Guid orderId, Order order)
    {
        _ordersDb[orderId] = order;
    }
}
