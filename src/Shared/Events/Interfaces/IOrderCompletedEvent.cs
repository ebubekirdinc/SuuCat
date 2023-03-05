namespace Shared.Events.Interfaces
{
    public interface IOrderCompletedEvent
    {
        public int OrderId { get; set; }
    }
}