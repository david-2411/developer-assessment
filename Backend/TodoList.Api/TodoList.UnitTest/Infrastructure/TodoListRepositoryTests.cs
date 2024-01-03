using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities;
using TodoList.Infrastructure.Persisitance;
using TodoList.Infrastructure;

namespace TodoList.UnitTest.Infrastructure
{
    public class TodoListRepositoryTests
    {
        [Fact]
        public async Task TodoListRepository_AddTodoItemAsync_ShouldAddTodoItem()
        {
            // Arrange
            using (var context = GetDbContext())
            {
                var repository = new TodoListRepository(context);
                var todoItem = new TodoItem { Id = Guid.NewGuid(), Description = "New Todo", IsCompleted = false };

                // Act
                await repository.AddTodoItemAsync(todoItem, CancellationToken.None);

                // Assert
                context.TodoItems.Should().Contain(todoItem);
            }
        }

        [Fact]
        public async Task TodoListRepository_GetTodoItemByIdAsync_ShouldReturnTodoItem()
        {
            // Arrange
            using (var context = GetDbContext())
            {
                var repository = new TodoListRepository(context);
                var existingTodoItem = new TodoItem { Id = Guid.NewGuid(), Description = "Existing Todo", IsCompleted = false };
                context.TodoItems.Add(existingTodoItem);
                context.SaveChanges();

                // Act
                var result = await repository.GetTodoItemByIdAsync(existingTodoItem.Id, CancellationToken.None);

                // Assert
                result.Should().BeEquivalentTo(existingTodoItem);
            }
        }

        [Fact]
        public async Task TodoListRepository_GetTodoItemByIdAsync_ShouldReturnNull()
        {
            // Arrange
            using (var context = GetDbContext())
            {
                var repository = new TodoListRepository(context);

                // Act
                var result = await repository.GetTodoItemByIdAsync(Guid.NewGuid(), CancellationToken.None);

                // Assert
                result.Should().BeNull();
            }
        }

        [Fact]
        public async Task TodoListRepository_UpdateTodoItemAsync_ShouldUpdateTodoItem()
        {
            // Arrange
            using (var context = GetDbContext())
            {
                var repository = new TodoListRepository(context);
                var todoItem = new TodoItem { Id = Guid.NewGuid(), Description = "Todo", IsCompleted = false };
                context.TodoItems.Add(todoItem);
                context.SaveChanges();

                // Act
                todoItem.IsCompleted = true;
                await repository.UpdateTodoItemAsync(todoItem, CancellationToken.None);

                // Assert
                context.Entry(todoItem).State.Should().Be(EntityState.Unchanged);
                context.TodoItems.Should().Contain(todoItem);
            }
        }

        [Fact]
        public async Task TodoListRepository_GetUncompletedTodoItemsAsync_ShouldReturnList()
        {
            // Arrange
            using (var context = GetDbContext())
            {
                var repository = new TodoListRepository(context);
                var uncompletedTodoItems = new List<TodoItem>
                {
                    new TodoItem { Id = Guid.NewGuid(), Description = "Todo 1", IsCompleted = false },
                    new TodoItem { Id = Guid.NewGuid(), Description = "Todo 2", IsCompleted = false }
                };
                var completedTodoItems = new List<TodoItem>
                {
                    new TodoItem { Id = Guid.NewGuid(), Description = "Todo 3", IsCompleted = true },
                    new TodoItem { Id = Guid.NewGuid(), Description = "Todo 4", IsCompleted = true }
                };

                context.TodoItems.AddRange(uncompletedTodoItems);
                context.TodoItems.AddRange(completedTodoItems);
                context.SaveChanges();

                // Act
                var result = await repository.GetUncompletedTodoItemsAsync(CancellationToken.None);

                // Assert
                result.Should().BeEquivalentTo(uncompletedTodoItems);
            }
        }

        [Fact]
        public void TodoListRepository_TodoItemDescriptionExists_ShouldReturnTrue()
        {
            // Arrange
            using (var context = GetDbContext())
            {
                var repository = new TodoListRepository(context);
                var existingTodoItem = new TodoItem { Id = Guid.NewGuid(), Description = "Existing Todo", IsCompleted = false };
                context.TodoItems.Add(existingTodoItem);
                context.SaveChanges();

                // Act
                var result = repository.TodoItemDescriptionExists(existingTodoItem.Description, CancellationToken.None);

                // Assert
                result.Should().BeTrue();
            }
        }

        [Fact]
        public void TodoListRepository_TodoItemDescriptionExists_ShouldReturnFalse()
        {
            // Arrange
            using (var context = GetDbContext())
            {
                var repository = new TodoListRepository(context);

                // Act
                var result = repository.TodoItemDescriptionExists("NonExisting Todo", CancellationToken.None);

                // Assert
                result.Should().BeFalse();
            }
        }

        [Fact]
        public void TodoListRepository_TodoItemIdExists_ShouldReturnTrue()
        {
            // Arrange
            using (var context = GetDbContext())
            {
                var repository = new TodoListRepository(context);
                var existingTodoItem = new TodoItem { Id = Guid.NewGuid(), Description = "Existing Todo", IsCompleted = false };
                context.TodoItems.Add(existingTodoItem);
                context.SaveChanges();

                // Act
                var result = repository.TodoItemIdExists(existingTodoItem.Id, CancellationToken.None);

                // Assert
                result.Should().BeTrue();
            }
        }

        [Fact]
        public void TodoListRepository_TodoItemIdExists_ShouldReturnFalse()
        {
            // Arrange
            using (var context = GetDbContext())
            {
                var repository = new TodoListRepository(context);

                // Act
                var result = repository.TodoItemIdExists(Guid.NewGuid(), CancellationToken.None);

                // Assert
                result.Should().BeFalse();
            }
        }

        private TodoContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new TodoContext(options);
        }
    }
}
