using EventBus.Events.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Domain.Enums;
using Order.Infrastructure.Persistence;

namespace Order.Infrastructure.Consumers;

public class OrderCompletedEventConsumer : IConsumer<IOrderCompletedEvent>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<OrderCompletedEventConsumer> _logger;

    public OrderCompletedEventConsumer(ApplicationDbContext context, ILogger<OrderCompletedEventConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IOrderCompletedEvent> context)
    {
        var order = await _context.Orders.FindAsync(context.Message.OrderId);

        if (order != null)
        {
            order.Status = OrderStatus.Complete;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order with Id: {MessageOrderId} completed successfully", context.Message.OrderId);
        }
        else
        {
            _logger.LogError("Order with Id: {MessageOrderId} not found", context.Message.OrderId);
        }
    }
}