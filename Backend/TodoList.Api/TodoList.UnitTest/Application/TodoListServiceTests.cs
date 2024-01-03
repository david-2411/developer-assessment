using TodoList.Application.Interfaces;
using TodoList.Application.Services.TodoList;
using TodoList.Domain.Entities;

namespace TodoList.UnitTest.Application
{
    public class TodoListServiceTests
    {
        private readonly ITodoListRepository _todoListRepository;
        public TodoListServiceTests() 
        {
            _todoListRepository = A.Fake<ITodoListRepository>();
        }

        [Fact]
        public async Task TodoListService_AddTodoItemAsync_ShouldAddTodoItem()
        {
            // Arrange
            var todoListService = new TodoListService(_todoListRepository);
            A.CallTo(() => _todoListRepository.TodoItemDescriptionExists(A<string>._, A<CancellationToken>._))
                .Returns(false);

            var todoItemId = Guid.NewGuid();
            var description = "New Todo";
            var isCompleted = false;

            // Act
            await todoListService.AddTodoItemAsync(todoItemId, description, isCompleted, CancellationToken.None);

            // Assert
            A.CallTo(() => _todoListRepository.AddTodoItemAsync(A<TodoItem>._, A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task TodoListService_AddTodoItemAsync_ShouldThrowException()
        {
            // Arrange
            var todoListService = new TodoListService(_todoListRepository);
            A.CallTo(() => _todoListRepository.TodoItemDescriptionExists(A<string>._, A<CancellationToken>._))
                .Returns(true);

            var todoItemId = Guid.NewGuid();
            var description = "Existing Todo";
            var isCompleted = false;

            // Act
            Func<Task> act = async () => 
                await todoListService.AddTodoItemAsync(todoItemId, description, isCompleted, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Description exists in database");
            A.CallTo(() => _todoListRepository.AddTodoItemAsync(A<TodoItem>._, A<CancellationToken>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task TodoListService_GetTodoItemAsync_ShouldReturnTodoItem()
        {
            // Arrange
            var todoListService = new TodoListService(_todoListRepository);
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Description = "Existing Todo", IsCompleted = false };
            A.CallTo(() => _todoListRepository.GetTodoItemByIdAsync(A<Guid>._, A<CancellationToken>._))
                .Returns(todoItem);

            var todoItemId = todoItem.Id;

            // Act
            var result = await todoListService.GetTodoItemAsync(todoItemId, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<TodoItem>();
            result.Should().BeEquivalentTo(todoItem);
        }

        [Fact]
        public async Task TodoListService_GetTodoItemAsync_ShouldReturnNull()
        {
            // Arrange
            var todoListService = new TodoListService(_todoListRepository);
            A.CallTo(() => _todoListRepository.GetTodoItemByIdAsync(A<Guid>._, A<CancellationToken>._))
                .Returns((TodoItem)null);

            var nonExistingTodoItemId = Guid.NewGuid();

            // Act
            var result = await todoListService.GetTodoItemAsync(nonExistingTodoItemId, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task TodoListService_GetUncompletedTodoItemsAsync_ShouldReturnList()
        {
            // Arrange
            var todoListService = new TodoListService(_todoListRepository);
            var todoItemList = A.Fake<List<TodoItem>>();
            A.CallTo(() => _todoListRepository.GetUncompletedTodoItemsAsync(A<CancellationToken>._))
                .Returns(todoItemList);

            // Act
            var result = await todoListService.GetUncompletedTodoItemsAsync(CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(todoItemList);
        }

        [Fact]
        public async Task TodoListService_UpdateTodoItemAsync_ShouldUpdateTodoItem()
        {
            // Arrange
            var todoListService = new TodoListService(_todoListRepository);
            A.CallTo(() => _todoListRepository.TodoItemIdExists(A<Guid>._, A<CancellationToken>._))
                .Returns(true);

            var existingTodoItemId = Guid.NewGuid();
            var updatedDescription = "Updated Todo";
            var updatedIsCompleted = true;

            // Act
            await todoListService.UpdateTodoItemAsync(existingTodoItemId, updatedDescription, updatedIsCompleted, CancellationToken.None);

            // Assert
            A.CallTo(() => _todoListRepository.UpdateTodoItemAsync(A<TodoItem>._, A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task TodoListService_UpdateTodoItemAsync_ShouldThrowException()
        {
            // Arrange
            var todoListRepository = A.Fake<ITodoListRepository>();
            A.CallTo(() => todoListRepository.TodoItemIdExists(A<Guid>._, A<CancellationToken>._))
                .Returns(false);
            var todoListService = new TodoListService(todoListRepository);

            var nonExistingTodoItemId = Guid.NewGuid();
            var updatedDescription = "Updated Todo";
            var updatedIsCompleted = true;

            // Act
            Func<Task> act = async () =>
                await todoListService.UpdateTodoItemAsync(nonExistingTodoItemId, updatedDescription, updatedIsCompleted, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage($"TodoItem {nonExistingTodoItemId} not found.");
            A.CallTo(() => _todoListRepository.UpdateTodoItemAsync(A<TodoItem>._, A<CancellationToken>._))
                .MustNotHaveHappened();
        }
    }
}
