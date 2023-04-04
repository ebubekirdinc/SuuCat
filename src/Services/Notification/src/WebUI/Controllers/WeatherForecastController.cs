using Microsoft.AspNetCore.Mvc;
using Notification.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace WebUI.Controllers;

/// <summary>
/// Weather forecast controller
/// </summary>
[Area("WebApi")]
public class WeatherForecastController : ApiControllerBase
{
    
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// WeatherForecast Get
    /// </summary> 
    /// <returns></returns>
    [HttpGet, Route("~/WebApi/WeatherForecast/Get")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        _logger.LogInformation("WeatherForecast Get");
        
        return await Mediator.Send(new GetWeatherForecastsQuery());
    }
}
