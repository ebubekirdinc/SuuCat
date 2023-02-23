using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Order.Commands.CreateOrder;
using Shared.Dto;

namespace WebUI.Controllers;

/// <summary>
/// Orders controller
/// </summary>
[Area("WebApi")]
[Produces(MediaTypeNames.Application.Json)]
public class OrdersController : ApiControllerBase
{
    /// <summary>
    /// Creates an Order
    /// </summary> 
    /// <returns></returns>
    [HttpPost, Route("~/WebApi/Orders")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResult<string>>> CreateCategory([FromBody] CreateOrderCommand commandd)
    {
        var command = new CreateOrderCommand { CustomerId = 1.ToString(), PaymentAccountId = "account_id_12",
            OrderItems = new List<OrderItemDto> { new OrderItemDto { ProductId = 25, Count = 5, Price = 100 } } };
        return await Mediator.Send(command);
    }
}