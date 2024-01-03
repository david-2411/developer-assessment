using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Domain.Entities;

namespace TodoList.Application.Services.TodoList
{
    public interface ITodoListService
    {
        Task<List<TodoItem>> GetUncompletedTodoItemsAsync(CancellationToken cancellationToken);
        Task<TodoItem> GetTodoItemAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateTodoItemAsync(Guid id, string description, bool isCompleted, CancellationToken cancellationToken);
        Task AddTodoItemAsync(Guid id, string description, bool isCompleted, CancellationToken cancellationToken);
    }
}
