using System.Net.Mime;
using Assessment.Application.MainCategories.Commands.CreateMainCategory;
using Assessment.Application.MainCategories.Commands.DeleteMainCategory;
using Assessment.Application.MainCategories.Commands.UpdateMainCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResult<CreateMainCategoryVm>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<CreateMainCategoryVm>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<CreateMainCategoryVm>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResult<CreateMainCategoryVm>>> CreateMainCategory(CreateMainCategoryCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Updates MainCategory by given id
    /// </summary> 
    /// <returns></returns>
    [HttpPatch, Route("~/AdminApi/MainCategories/{mainCategoryId:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResult<UpdateMainCategoryVm>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<UpdateMainCategoryVm>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<UpdateMainCategoryVm>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResult<UpdateMainCategoryVm>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResult<UpdateMainCategoryVm>>> UpdateMainCategory(UpdateMainCategoryCommand command, int mainCategoryId)
    {
        command.Id = mainCategoryId;

        return await Mediator.Send(command);
    }

    /// <summary>
    /// Deletes MainCategory by given id
    /// </summary> 
    /// <returns></returns>
    [HttpDelete, Route("~/AdminApi/MainCategories/{mainCategoryId:int}")]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResult<string>>> UpdateMainCategory(int mainCategoryId)
    {
        return await Mediator.Send(new DeleteMainCategoryCommand { Id = mainCategoryId });
    }
}