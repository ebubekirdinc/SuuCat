using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events.Interfaces;

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

        _logger.LogInformation($"Order (Id={context.Message.OrderId}) Order Completed notification sent");
    }
}