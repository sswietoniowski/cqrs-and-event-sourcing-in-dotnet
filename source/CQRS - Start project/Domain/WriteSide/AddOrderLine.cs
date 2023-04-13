namespace Domain.WriteSide
{
    public class AddOrderLine : ICommand
    {
        public Guid Id { get; set; }
        public OrderLine OrderLine { get; set; }
    }
}
