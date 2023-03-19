using MediatR;
using Shared.Dto;

namespace Order.Application.Order.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<ApiResult<string>>
{
    public CreateOrderCommand()
    {
        OrderItemList = new List<OrderItemDto>();
    }
    public string CustomerId { get; set; }
    public string PaymentAccountId { get; set; }
    public List<OrderItemDto> OrderItemList { get; set; }
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
}