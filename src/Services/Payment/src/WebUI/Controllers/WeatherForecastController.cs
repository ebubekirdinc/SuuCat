using Microsoft.AspNetCore.Mvc;
using Payment.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace WebUI.Controllers;

/// <summary>
/// Weather forecast controller
/// </summary>
[Area("WebApi")]
public class WeatherForecastController : ApiControllerBase
{
    /// <summary>
    /// WeatherForecast Get
    /// </summary> 
    /// <returns></returns>
    [HttpGet, Route("~/WebApi/WeatherForecast/Get")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        return await Mediator.Send(new GetWeatherForecastsQuery());
    }
}
