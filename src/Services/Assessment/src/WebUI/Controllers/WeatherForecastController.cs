using Assessment.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.WebUI.Controllers;

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
    [Authorize]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        return await Mediator.Send(new GetWeatherForecastsQuery());
    }
}
