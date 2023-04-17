namespace Domain;

public interface IOrderServiceReadSide
{
    // Queries
    Order LoadOrder(Guid orderId);
    List<OrderSummary> LoadAllOrders();
}