using MediatR;
using Shared.Dto;

namespace Subscription.Application.Stock.Commands.AddStock;

public class AddStockCommand : IRequest<ApiResult<string>>
{ 
    public int ProductId { get; set; }
    public int Amount { get; set; } 
}
 