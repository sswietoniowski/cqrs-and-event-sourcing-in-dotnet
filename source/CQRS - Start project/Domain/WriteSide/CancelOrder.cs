namespace Domain.WriteSide
{
    public class CancelOrder : ICommand
    {
        public Guid Id { get; set; }
    }
}
