using Assessment.Application.TodoLists.Queries.ExportTodos;

namespace Assessment.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
