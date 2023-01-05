﻿using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Assessment.WebUI.Controllers;

[ApiController]
[Route("[area]/[controller]/[action]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
