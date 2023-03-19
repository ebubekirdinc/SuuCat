using EventBus.Events.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Domain.Enums;
using Order.Infrastructure.Persistence;

namespace Order.Infrastructure.Consumers;

public class OrderFailedEventConsumer : IConsumer<IOrderFailedEvent>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<OrderFailedEventConsumer> _logger;

    public OrderFailedEventConsumer(ApplicationDbContext context, ILogger<OrderFailedEventConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IOrderFailedEvent> context)
    {
        var order = await _context.Orders.FindAsync(context.Message.OrderId);

        if (order != null)
        {
            order.Status = OrderStatus.Fail;
            order.ErrorMessage = context.Message.ErrorMessage;
            
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order with Id: {MessageOrderId} failed, status updated to {Status}", context.Message.OrderId, OrderStatus.Fail);
        }
        else
        {
            _logger.LogError("Order with Id: {MessageOrderId} not found", context.Message.OrderId);
        }
    }
}