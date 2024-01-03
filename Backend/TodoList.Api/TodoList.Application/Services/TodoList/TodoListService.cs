using System.Linq.Expressions;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;

namespace TodoList.Application.Services.TodoList
{
    public class TodoListService : ITodoListService
    {
        private readonly ITodoListRepository _todoListRepository;

        public TodoListService(ITodoListRepository todoListRepository)
        {
            _todoListRepository = todoListRepository;
        }

        public async Task AddTodoItemAsync(Guid id, string description, bool isCompleted, CancellationToken cancellationToken)
        {
            if(_todoListRepository.TodoItemDescriptionExists(description, cancellationToken) == true)
            {
                throw new Exception("Description exists in database");
            }

            TodoItem todoItem = new TodoItem()
            {
                Id = id,
                Description = description,
                IsCompleted = isCompleted
            };
            await _todoListRepository.AddTodoItemAsync(todoItem, cancellationToken);
        }

        public async Task<TodoItem> GetTodoItemAsync(Guid id, CancellationToken cancellationToken)
        {
            var todoItem = await _todoListRepository.GetTodoItemByIdAsync(id, cancellationToken);
            return todoItem;
        }

        public async Task<List<TodoItem>> GetUncompletedTodoItemsAsync(CancellationToken cancellationToken)
        {
            var todoItems = await _todoListRepository.GetUncompletedTodoItemsAsync(cancellationToken);
            return todoItems;
        }

        public async Task UpdateTodoItemAsync(Guid id, string description, bool isCompleted, CancellationToken cancellationToken)
        {
            if (!_todoListRepository.TodoItemIdExists(id, cancellationToken))
            {
                throw new Exception($"TodoItem {id} not found.");
            }

            TodoItem toDoItem = new TodoItem()
            {
                Id = id,
                Description = description,
                IsCompleted = isCompleted,
            };
            await _todoListRepository.UpdateTodoItemAsync(toDoItem, cancellationToken);
        }
    }
}
 