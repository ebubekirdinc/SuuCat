using EventBus.Events.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Notification.Infrastructure.Consumers;

public class OrderFailedEventConsumer : IConsumer<IOrderFailedEvent>
{
    private readonly ILogger<OrderFailedEventConsumer> _logger;

    public OrderFailedEventConsumer(ILogger<OrderFailedEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IOrderFailedEvent> context)
    {
        // TODO: Send email to customer

        _logger.LogInformation("Order Failed notification sent to customer with Id: {CustomerId} for order Id: {MessageOrderId}",
            context.Message.CustomerId, context.Message.OrderId);
    }
}