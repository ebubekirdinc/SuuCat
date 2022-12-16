using Account.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Account.WebUI.Controllers;

public class WeatherForecastController : ApiControllerBase
{
    [Area("WebApi")]
    [HttpGet, Route("~/WebApi/WeatherForecast/Get")]
    // [Authorize]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        return await Mediator.Send(new GetWeatherForecastsQuery());
    }
}
