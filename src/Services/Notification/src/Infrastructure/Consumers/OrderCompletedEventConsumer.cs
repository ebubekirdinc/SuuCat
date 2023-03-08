using EventBus.Events.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Notification.Infrastructure.Consumers;

public class OrderCompletedEventConsumer : IConsumer<IOrderCompletedEvent>
{
    private readonly ILogger<OrderCompletedEventConsumer> _logger;

    public OrderCompletedEventConsumer(ILogger<OrderCompletedEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IOrderCompletedEvent> context)
    {
        // TODO: Send email to customer
        
        _logger.LogInformation($"Order (Id={context.Message.OrderId}) Order Completed notification sent");
    }
}