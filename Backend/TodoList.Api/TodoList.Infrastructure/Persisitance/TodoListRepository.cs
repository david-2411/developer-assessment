using Microsoft.EntityFrameworkCore;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;

namespace TodoList.Infrastructure.Persisitance
{
    public class TodoListRepository : ITodoListRepository
    {
        private readonly TodoContext _context;
        public TodoListRepository(TodoContext context)
        {
            _context = context;
        }
        public async Task AddTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken)
        {
            await _context.TodoItems.AddAsync(todoItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<TodoItem> GetTodoItemByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task UpdateTodoItemAsync(TodoItem item, CancellationToken cancellationToken)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<List<TodoItem>> GetUncompletedTodoItemsAsync(CancellationToken cancellationToken)
        {
            return await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
        }

        public bool TodoItemDescriptionExists(string description, CancellationToken cancellationToken)
        {
            return _context.TodoItems
                   .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }

        public bool TodoItemIdExists(Guid id, CancellationToken cancellationToken)
        {
            return _context.TodoItems.Any(x => x.Id == id);
        }

        
    }
}
