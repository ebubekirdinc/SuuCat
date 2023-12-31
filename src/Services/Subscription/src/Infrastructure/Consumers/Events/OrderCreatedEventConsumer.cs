using EventBus.Events;
using EventBus.Events.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Subscription.Application.Common.Interfaces.MassTransit;
using Subscription.Infrastructure.Persistence;
using Tracing;

namespace Subscription.Infrastructure.Consumers.Events;

public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderCreatedEventConsumer> _logger;
    private readonly IMassTransitService _massTransitService;

    public OrderCreatedEventConsumer(ApplicationDbContext context, ILogger<OrderCreatedEventConsumer> logger, IMassTransitService massTransitService)
    {
        _massTransitService = massTransitService;
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IOrderCreatedEvent> context)
    {
        var isThereEnoughStock = true;
        foreach (var item in _context.Stocks.Where(x => context.Message.OrderItemList.Select(y => y.ProductId).Contains(x.ProductId)).AsEnumerable())
        {
            if (!context.Message.OrderItemList.Select(y => y.ProductId).Contains(item.ProductId) || item.Count <= context.Message.OrderItemList.FirstOrDefault(y => y.ProductId == item.ProductId).Count)
            {
                isThereEnoughStock = false;
                break;
            }
        }

        if (!isThereEnoughStock)
        {
            await _massTransitService.Publish(new StockReservationFailedEvent
            {
                CorrelationId = context.Message.CorrelationId,
                ErrorMessage = "Not enough stock"
            });

            _logger.LogInformation("Not enough stock for CorrelationId Id :{MessageCorrelationId}", context.Message.CorrelationId);
        }
        else
        {
            foreach (var item in context.Message.OrderItemList)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                if (stock == null)
                {
                    await _massTransitService.Publish(new StockReservationFailedEvent
                    {
                        CorrelationId = context.Message.CorrelationId,
                        ErrorMessage = $"Stock not found with product id {item.ProductId} and CorrelationId Id :{context.Message.CorrelationId}"
                    });

                    _logger.LogInformation("Stock not found with product Id: {ItemProductId} and CorrelationId Id :{MessageCorrelationId}", item.ProductId, context.Message.CorrelationId);
                    return;
                }

                stock.Count -= item.Count;
                await _context.SaveChangesAsync();
                
                OpenTelemetryMetric.StockCounter.Add(-item.Count);
            }

            _logger.LogInformation("Stock was reserved with CorrelationId Id: {MessageCorrelationId}", context.Message.CorrelationId);

            StockReservedEvent stockReservedEvent = new StockReservedEvent
            {
                CorrelationId = context.Message.CorrelationId,
                OrderItemList = context.Message.OrderItemList
            };

            await _massTransitService.Publish(stockReservedEvent);
        }
    }
}