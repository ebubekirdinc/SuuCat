using System.Net.Mime;
using Microsoft.AspNetCore.Mvc; 
using Shared.Dto;
using Subscription.Application.Stock.Commands.AddStock;
using Tracing;

namespace WebUI.Controllers;

/// <summary>
/// Stocks controller
/// </summary>
[Area("WebApi")]
[Produces(MediaTypeNames.Application.Json)]
public class StocksController : ApiControllerBase
{
    private readonly ILogger<StocksController> _logger;    

    public StocksController(ILogger<StocksController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Increases the stock of a product
    /// </summary> 
    /// <returns></returns>
    [HttpPost, Route("~/WebApi/Stocks")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResult<string>>> AddStock([FromBody] AddStockCommand command)
    {
        
        return await Mediator.Send(command);
    }
}