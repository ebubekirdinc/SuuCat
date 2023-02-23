using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Domain.Enums;
using Order.Infrastructure.Persistence;
using Shared.Interfaces;

namespace Order.Infrastructure.Consumers;

public class OrderRequestCompletedEventConsumer : IConsumer<IOrderRequestCompletedEvent>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<OrderRequestCompletedEventConsumer> _logger;

    public OrderRequestCompletedEventConsumer(ApplicationDbContext context, ILogger<OrderRequestCompletedEventConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IOrderRequestCompletedEvent> context)
    {
        var order = await _context.Orders.FindAsync(context.Message.OrderId);

        if (order != null)
        {
            order.Status = OrderStatus.Complete;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order (Id={context.Message.OrderId}) status changed : {order.Status}");
        }
        else
        {
            _logger.LogError($"Order (Id={context.Message.OrderId}) not found");
        }
    }
}