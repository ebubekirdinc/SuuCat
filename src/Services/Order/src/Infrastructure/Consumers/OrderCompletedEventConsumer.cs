using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Domain.Enums;
using Order.Infrastructure.Persistence;
using Shared.Events.Interfaces;

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

            _logger.LogInformation($"Order (Id={context.Message.OrderId}) status changed : {order.Status}");
        }
        else
        {
            _logger.LogError($"Order (Id={context.Message.OrderId}) not found");
        }
    }
}