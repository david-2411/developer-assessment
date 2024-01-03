using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using TodoList.Api.Controllers;
using TodoList.Application.Services.TodoList;
using TodoList.Domain.Entities;
using Xunit;

namespace TodoList.UnitTests
{
    public class TodoItemsControllerTests
    {
        //private readonly ITodoListService _todoListService;
        //private readonly ILogger<TodoItemsController> _logger;
        public TodoItemsControllerTests() 
        {
            //_todoListService = A.Fake<ITodoListService>();
            //_logger = A.Fake<ILogger<TodoItemsController>>();
        }

        [Fact]
        public void TodoItemsController_GetTodoItems_ReturnOk()
        {/*
            //Arrange
            var todoItemsList = A.Fake<List<TodoItem>>();
            var tcs = new CancellationTokenSource(1000);
            //A.CallTo(() => _todoListService.GetUncomletedTodoItemsAsync(tcs.Token)).Returns(todoItemsList);
            var controller = new TodoItemsController(_todoListService,_logger);
            //Act
            var result = controller.GetTodoItems(tcs.Token);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        */}
    }
}
