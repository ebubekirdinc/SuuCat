using AutoMapper;
using EventBus.Constants;
using EventBus.Messages;
using EventBus.Messages.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Common.Interfaces;
using Order.Application.Common.Interfaces.MassTransit;
using Order.Domain.Enums;
using Shared.Dto;
using OrderItem = EventBus.Events.OrderItem;

namespace Order.Application.Order.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<string>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMassTransitService _massTransitService;
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(IApplicationDbContext context, IMapper mapper, IMassTransitService massTransitService, ILogger<CreateOrderCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _massTransitService = massTransitService;
        _logger = logger;
    }

    public async Task<ApiResult<string>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var newOrder = new Domain.Entities.Order
        {
            CustomerId = request.CustomerId,
            Status = OrderStatus.Pending,
            PaymentAccountId = request.PaymentAccountId
        };

        request.OrderItems.ForEach(item =>
        {
            newOrder.OrderItems.Add(new Domain.Entities.OrderItem { Price = item.Price, ProductId = item.ProductId, Count = item.Count, });
        });

        await _context.Orders.AddAsync(newOrder, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken); 
        
        var createOrderMessage = new CreateOrderMessage()
        {
            CustomerId = request.CustomerId,
            OrderId = newOrder.Id,
            PaymentAccountId =request.PaymentAccountId,
            TotalPrice = newOrder.OrderItems.Sum(x => x.Price * x.Count)
        };

        newOrder.OrderItems.ForEach(item =>
        {
            createOrderMessage.OrderItems.Add(new OrderItem { Count = item.Count, ProductId = item.ProductId });
        });
 
        await _massTransitService.Send<ICreateOrderMessage>(createOrderMessage, QueuesConsts.CreateOrderMessageQueueName);
        _logger.LogInformation($"Order (Id={newOrder.Id}) created successfully");
        

        return new ApiResult<string>(true, "Order created successfully");
    }
}