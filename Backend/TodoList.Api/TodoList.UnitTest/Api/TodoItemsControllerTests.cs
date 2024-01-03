using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoList.Api.Controllers;
using TodoList.Application.Services.TodoList;
using TodoList.Contracts;
using TodoList.Domain.Entities;

namespace TodoList.UnitTest.Api
{
    public class TodoItemsControllerTests
    {
        private readonly ILogger<TodoItemsController> _logger;
        private readonly ITodoListService _todoListService;

        public TodoItemsControllerTests()
        {
            _logger = A.Fake<ILogger<TodoItemsController>>();
            _todoListService = A.Fake<ITodoListService>();
        }

        [Fact]
        public async Task TodoItemsController_GetTodoItems_ShouldReturnOk()
        {
            //Arrange
            var controller = new TodoItemsController(_todoListService, _logger);
            var todoItems = A.Fake<List<TodoItem>>();
            var cts = new CancellationTokenSource(1000);
            A.CallTo(() => _todoListService.GetUncompletedTodoItemsAsync(cts.Token))
                .Returns(Task.FromResult(todoItems));
            //Act
            var result = await controller.GetTodoItems(cts.Token);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async Task TodoItemsController_GetTodoItem_ShouldReturnsOkWithTodoItem()
        {
            // Arrange
            var controller = new TodoItemsController(_todoListService, _logger);
            var id = Guid.NewGuid();
            var todoItem = new TodoItem { Id = id, Description = "Fake Todo", IsCompleted = false };
            var cts = new CancellationTokenSource(1000);
            A.CallTo(() => _todoListService.GetTodoItemAsync(id, cts.Token))
                .Returns(Task.FromResult(todoItem));

            // Act
            var result = await controller.GetTodoItem(id, cts.Token);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().NotBeNull().And.BeEquivalentTo(todoItem);
        }

        [Fact]
        public async Task TodoItemsController_GetTodoItem_ShouldReturnsNotFound()
        {
            // Arrange
            var controller = new TodoItemsController(_todoListService, _logger);
            var id = Guid.NewGuid();
            var cts = new CancellationTokenSource(1000);
            A.CallTo(() => _todoListService.GetTodoItemAsync(id, cts.Token))
                .Returns(Task.FromResult<TodoItem>(null));

            // Act
            var result = await controller.GetTodoItem(id, cts.Token);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task TodoItemsController_PostTodoItem_ShouldReturnsCreatedAtAction()
        {
            // Arrange
            var controller = new TodoItemsController(_todoListService, _logger);
            var todoItem = new TodoItemDto { Id = Guid.NewGuid(), Description = "Fake Todo", IsCompleted = false };
            var cts = new CancellationTokenSource(1000);
            A.CallTo(() => _todoListService.AddTodoItemAsync(todoItem.Id, todoItem.Description, todoItem.IsCompleted, cts.Token))
                .Returns(Task.FromResult<TodoItem>(null));

            // Act
            var result = await controller.PostTodoItem(todoItem, cts.Token);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdAtActionResult = (CreatedAtActionResult)result;
            createdAtActionResult.ActionName.Should().Be(nameof(controller.GetTodoItem));
            createdAtActionResult.RouteValues["id"].Should().Be(todoItem.Id);
            createdAtActionResult.Value.Should().BeEquivalentTo(todoItem);
        }

        [Fact]
        public async Task TodoItemsController_PostTodoItem_ShouldReturnsBadRequest()
        {
            // Arrange
            var controller = new TodoItemsController(_todoListService, _logger);
            var id = Guid.NewGuid();
            var invalidTodoItem = new TodoItemDto { Id = Guid.NewGuid(), Description = null, IsCompleted = false };
            var cts = new CancellationTokenSource(1000);

            // Act
            var result = await controller.PostTodoItem(invalidTodoItem, cts.Token);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            A.CallTo(() => _todoListService.AddTodoItemAsync(A<Guid>._, A<string>._, A<bool>._, A<CancellationToken>._))
                .MustNotHaveHappened();
        }
    }
}
