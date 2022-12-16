using Account.Application.TodoLists.Queries.ExportTodos;

namespace Account.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
