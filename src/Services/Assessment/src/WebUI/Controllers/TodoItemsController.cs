// using Assessment.Application.Common.Models;
// using Assessment.Application.TodoItems.Commands.CreateTodoItem;
// using Assessment.Application.TodoItems.Commands.DeleteTodoItem;
// using Assessment.Application.TodoItems.Commands.UpdateTodoItem;
// using Assessment.Application.TodoItems.Commands.UpdateTodoItemDetail;
// using Assessment.Application.TodoItems.Queries.GetTodoItemsWithPagination;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Assessment.WebUI.Controllers;
//
// [Authorize]
// public class TodoItemsController : ApiControllerBase
// {
//     [HttpGet]
//     public async Task<ActionResult<PaginatedList<TodoItemBriefDto>>> GetTodoItemsWithPagination([FromQuery] GetTodoItemsWithPaginationQuery query)
//     {
//         return await Mediator.Send(query);
//     }
//
//     [HttpPost]
//     public async Task<ActionResult<int>> Create(CreateTodoItemCommand command)
//     {
//         return await Mediator.Send(command);
//     }
//
//     [HttpPut("{id}")]
//     public async Task<ActionResult> Update(int id, UpdateTodoItemCommand command)
//     {
//         if (id != command.Id)
//         {
//             return BadRequest();
//         }
//
//         await Mediator.Send(command);
//
//         return NoContent();
//     }
//
//     [HttpPut("[action]")]
//     public async Task<ActionResult> UpdateItemDetails(int id, UpdateTodoItemDetailCommand command)
//     {
//         if (id != command.Id)
//         {
//             return BadRequest();
//         }
//
//         await Mediator.Send(command);
//
//         return NoContent();
//     }
//
//     [HttpDelete("{id}")]
//     public async Task<ActionResult> Delete(int id)
//     {
//         await Mediator.Send(new DeleteTodoItemCommand(id));
//
//         return NoContent();
//     }
// }
