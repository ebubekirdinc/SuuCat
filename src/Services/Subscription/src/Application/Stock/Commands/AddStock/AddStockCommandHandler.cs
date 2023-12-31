using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Dto;
using Subscription.Application.Common.Interfaces;
using Subscription.Application.Common.Interfaces.MassTransit;
using Tracing;

namespace Subscription.Application.Stock.Commands.AddStock;

public class AddStockCommandHandler : IRequestHandler<AddStockCommand, ApiResult<string>>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<AddStockCommandHandler> _logger;

    public AddStockCommandHandler(IApplicationDbContext context, ILogger<AddStockCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResult<string>> Handle(AddStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == request.ProductId, cancellationToken: cancellationToken);

        if (stock == null)
        {
            _logger.LogInformation("Stock not found. Product Id: {ProductId}", request.ProductId);

            return new ApiResult<string>(false, "Stock not found");
        }

        stock.Count += request.Amount;

        await _context.SaveChangesAsync(cancellationToken);
        
        OpenTelemetryMetric.StockCounter.Add(request.Amount);

        _logger.LogInformation("Successfully added stock to product with Id: {ProductId}", request.ProductId);

        return new ApiResult<string>(true, "Stock added successfully");
    }
}