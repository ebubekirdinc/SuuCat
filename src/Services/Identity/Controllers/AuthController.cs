using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using EventBus.Constants;
using EventBus.Events;
using Identity.Models;
using IdentityServer4;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Shared.Dto;
using Tracing;

namespace Identity.Controllers;

[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
// [Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IPublishEndpoint _publishEndpoint;

    public AuthController(UserManager<ApplicationUser> userManager, IPublishEndpoint publishEndpoint)
    {
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost, Route("~/Api/Auth/SignUp")]
    [AllowAnonymous]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp(SignupDto signupDto)
    {
        using var activity = ActivitySourceProvider.Source.StartActivity();
        activity?.AddEvent(new("User creation started."));
        
        var user = new ApplicationUser
        {
            UserName = signupDto.Email,
            Email = signupDto.Email,
        };

        var result = await _userManager.CreateAsync(user, signupDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(x => x.Description).ToList());
        }

        await _publishEndpoint.Publish<UserCreatedEvent>(new UserCreatedEvent
        {
            UserId = user.Id,
            Email = user.Email,
            UserName = user.UserName
        });
        
        OpenTelemetryMetric.UserCreatedEventCounter.Add(1, new KeyValuePair<string, object>("event.name", "UserCreatedEvent"));
        
        activity?.AddEvent(new("User created successfully."));
        
        return Ok(new ApiResult<string>(true, "User created successfully"));;
    }

    [HttpGet, Route("~/Api/Auth/GetUser")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser()
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

        if (userIdClaim == null) return BadRequest();

        var user = await _userManager.FindByIdAsync(userIdClaim.Value);

        if (user == null) return BadRequest();

        return Ok(new { Id = user.Id, UserName = user.UserName, Email = user.Email });
    }

    [Authorize(Roles = "admin")]
    [HttpGet, Route("~/Api/Auth/OnlyAdmin")]
    public async Task<IActionResult> OnlyAdmin()
    {
        await LogTokenAndClaims();

        return Ok();
    }

    private async Task LogTokenAndClaims()
    {
        var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

        Debug.WriteLine($"Identity token: {identityToken}");

        foreach (var claim in User.Claims)
        {
            Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
        }
    }
}