using System.Net.Mime; 
using Assessment.Application.Categories.Commands.CreateCategory;
using Assessment.Application.Categories.Commands.DeleteCategory;
using Assessment.Application.Categories.Commands.UpdateCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dto;

namespace WebUI.Controllers.AdminApi;

/// <summary>
/// Categories controller
/// </summary>
[Area("AdminApi")]
[Authorize(Roles = "admin")]
[Produces(MediaTypeNames.Application.Json)]
public class CategoriesController : ApiControllerBase
{
    /// <summary>
    /// Creates a Category
    /// </summary> 
    /// <returns></returns>
    [HttpPost, Route("~/AdminApi/Categories")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResult<CreateCategoryVm>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<CreateCategoryVm>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<CreateCategoryVm>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResult<CreateCategoryVm>>> CreateCategory(CreateCategoryCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Updates Category by given id
    /// </summary> 
    /// <returns></returns>
    [HttpPatch, Route("~/AdminApi/Categories/{categoryId:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResult<UpdateCategoryVm>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<UpdateCategoryVm>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<UpdateCategoryVm>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResult<UpdateCategoryVm>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResult<UpdateCategoryVm>>> UpdateCategory(UpdateCategoryCommand command, int categoryId)
    {
        command.Id = categoryId;

        return await Mediator.Send(command);
    }

    /// <summary>
    /// Deletes Category by given id
    /// </summary> 
    /// <returns></returns>
    [HttpDelete, Route("~/AdminApi/Categories/{categoryId:int}")]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResult<string>>> UpdateCategory(int categoryId)
    {
        return await Mediator.Send(new DeleteCategoryCommand { Id = categoryId });
    }
}