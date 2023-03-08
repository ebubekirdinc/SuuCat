namespace EventBus.Events
{
    public class UserCreatedEvent
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}