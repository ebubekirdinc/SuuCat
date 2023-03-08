using EventBus.Messages.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Subscription.Infrastructure.Persistence;

namespace Subscription.Infrastructure.Consumers.Messages;

public class StockRollBackMessageConsumer : IConsumer<IStockRollBackMessage>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<StockRollBackMessageConsumer> _logger;

    public StockRollBackMessageConsumer(ApplicationDbContext context, ILogger<StockRollBackMessageConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IStockRollBackMessage> context)
    {
        foreach (var item in context.Message.OrderItems)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

            if (stock != null)
            {
                stock.Count += item.Count;
                await _context.SaveChangesAsync();
            }
        }

        _logger.LogInformation($"Stock was released");
    }
}