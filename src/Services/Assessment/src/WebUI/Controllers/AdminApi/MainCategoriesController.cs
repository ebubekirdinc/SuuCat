using System.Diagnostics;
using System.Net.Mime;
using Assessment.Application.MainCategories.Commands.CreateMainCategory;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Shared.Dto;

namespace WebUI.Controllers.AdminApi;

/// <summary>
/// MainCategories controller
/// </summary>
[Area("AdminApi")] 
[Authorize(Roles = "admin")]
[Produces(MediaTypeNames.Application.Json)]
public class MainCategoriesController : ApiControllerBase
{
    /// <summary>
    /// Creates a MainCategory
    /// </summary> 
    /// <returns></returns>
    [HttpPost, Route("~/AdminApi/MainCategories")]
    [ProducesResponseType(typeof(ApiResult<CreateMainCategoryVm>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<CreateMainCategoryVm>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<CreateMainCategoryVm>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResult<CreateMainCategoryVm>>> CreateMainCategory(CreateMainCategoryCommand command)
    { 
        return await Mediator.Send(command);
    }
    
 


   
}