using TodoList.Domain.Entities;

namespace TodoList.Application.Interfaces
{
    public interface ITodoListRepository
    {
        Task<List<TodoItem>> GetUncompletedTodoItemsAsync(CancellationToken cancellationToken);
        Task<TodoItem> GetTodoItemByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateTodoItemAsync(TodoItem item, CancellationToken cancellationToken);
        Task AddTodoItemAsync(TodoItem item, CancellationToken cancellationToken);
        bool TodoItemIdExists(Guid id, CancellationToken cancellationToken);
        bool TodoItemDescriptionExists(string description, CancellationToken cancellationToken);
    }
}
