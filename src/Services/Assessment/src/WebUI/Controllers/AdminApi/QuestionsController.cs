using System.Net.Mime; 
using Assessment.Application.Questions.Commands.CreateQuestion;
using Assessment.Application.Questions.Commands.DeleteCategory;
using Assessment.Application.Questions.Commands.UpdateQuestion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dto;

namespace WebUI.Controllers.AdminApi;

/// <summary>
/// Questions controller
/// </summary>
[Area("AdminApi")]
[Authorize(Roles = "admin")]
[Produces(MediaTypeNames.Application.Json)]
public class QuestionsController : ApiControllerBase
{
    /// <summary>
    /// Creates a Question
    /// </summary> 
    /// <returns></returns>
    [HttpPost, Route("~/AdminApi/Questions")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResult<CreateQuestionVm>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<CreateQuestionVm>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<CreateQuestionVm>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResult<CreateQuestionVm>>> CreateQuestion(CreateQuestionCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Updates Question by given id
    /// </summary> 
    /// <returns></returns>
    [HttpPatch, Route("~/AdminApi/Questions/{questionId:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResult<UpdateQuestionVm>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<UpdateQuestionVm>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<UpdateQuestionVm>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResult<UpdateQuestionVm>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResult<UpdateQuestionVm>>> UpdateQuestion(UpdateQuestionCommand command, int questionId)
    {
        command.Id = questionId;
    
        return await Mediator.Send(command);
    }
    
    /// <summary>
    /// Deletes Question by given id
    /// </summary> 
    /// <returns></returns>
    [HttpDelete, Route("~/AdminApi/Questions/{questionId:int}")]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResult<string>>> UpdateQuestion(int questionId)
    {
        return await Mediator.Send(new DeleteQuestionCommand { Id = questionId });
    }
}