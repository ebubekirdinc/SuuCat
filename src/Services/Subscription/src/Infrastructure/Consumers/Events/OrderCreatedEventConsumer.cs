using EventBus.Events;
using EventBus.Events.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Subscription.Application.Common.Interfaces.MassTransit;
using Subscription.Infrastructure.Persistence;

namespace Subscription.Infrastructure.Consumers.Events;

public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderCreatedEventConsumer> _logger;
    private readonly IMassTransitService _massTransitService;

    public OrderCreatedEventConsumer(ApplicationDbContext context, ILogger<OrderCreatedEventConsumer> logger, IMassTransitService massTransitService)
    {
        _context = context;
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task  Consume(ConsumeContext<IOrderCreatedEvent> context)
    {
        var stockResult = new List<bool>();

        foreach (var item in context.Message.OrderItems)
        {
            stockResult.Add(await _context.Stocks.AnyAsync(x => x.ProductId == item.ProductId && x.Count > item.Count));
        }

        if (stockResult.All(x => x.Equals(true)))
        {
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                if (stock != null)
                {
                    stock.Count -= item.Count;
                }

                await _context.SaveChangesAsync();
            }

            _logger.LogInformation($"Stock was reserved for CorrelationId Id :{context.Message.CorrelationId}");

            StockReservedEvent stockReservedEvent = new StockReservedEvent(context.Message.CorrelationId)
            {
                OrderItems = context.Message.OrderItems
            };

            await _massTransitService.Publish(stockReservedEvent);
        }
        else
        {
            await _massTransitService.Publish(new StockNotReservedEvent(context.Message.CorrelationId)
            {
                Reason = "Not enough stock"
            });
        
            _logger.LogInformation($"Not enough stock for CorrelationId Id :{context.Message.CorrelationId}");
        }
    }
}