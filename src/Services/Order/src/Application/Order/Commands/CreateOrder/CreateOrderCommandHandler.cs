using AutoMapper;
using MediatR;
using Order.Application.Common.Exceptions;
using Order.Application.Common.Interfaces;
using Order.Application.Common.Interfaces.MassTransit;
using Order.Domain.Entities;
using Order.Domain.Enums;
using Shared.Constants;
using Shared.Dto;
using Shared.Events;
using Shared.Interfaces;

namespace Order.Application.Order.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<string>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMassTransitService _massTransitService;

    public CreateOrderCommandHandler(IApplicationDbContext context, IMapper mapper, IMassTransitService massTransitService)
    {
        _context = context;
        _mapper = mapper;
        _massTransitService = massTransitService;
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
            newOrder.OrderItems.Add(new OrderItem { Price = item.Price, ProductId = item.ProductId, Count = item.Count, });
        });

        await _context.Orders.AddAsync(newOrder, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken); 
        
        var orderCreatedRequestEvent = new OrderCreatedRequestEvent()
        {
            CustomerId = request.CustomerId,
            OrderId = newOrder.Id,
            PaymentAccountId =request.PaymentAccountId,
            TotalPrice = newOrder.OrderItems.Sum(x => x.Price * x.Count)
        };

        newOrder.OrderItems.ForEach(item =>
        {
            orderCreatedRequestEvent.OrderItems.Add(new OrderCreatedRequestEventItem { Count = item.Count, ProductId = item.ProductId });
        });
 
        await _massTransitService.Send<IOrderCreatedRequestEvent>(orderCreatedRequestEvent, QueuesConsts.OrderCreatedRequest);
        

        return new ApiResult<string>(true, "Order created successfully");
    }
}