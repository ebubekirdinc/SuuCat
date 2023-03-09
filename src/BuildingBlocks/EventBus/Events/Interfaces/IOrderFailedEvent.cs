namespace EventBus.Events.Interfaces;

public interface IOrderFailedEvent
{
    public int OrderId { get; set; }
    public string Reason { get; set; }
}