namespace Domain.WriteSide
{
    public class CreateOrder : ICommand
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
