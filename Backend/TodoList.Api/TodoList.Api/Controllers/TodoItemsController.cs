using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Application.Services.TodoList;
using TodoList.Contracts;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ILogger<TodoItemsController> _logger;
        private readonly ITodoListService _todoListService;

        public TodoItemsController(ITodoListService todoListService, ILogger<TodoItemsController> logger)
        {
            _todoListService = todoListService;
            _logger = logger;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems(CancellationToken token)
        {
            var results = await _todoListService.GetUncompletedTodoItemsAsync(token);
            return Ok(results);
        }

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id, CancellationToken token)
        {
            var result = await _todoListService.GetTodoItemAsync(id, token);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/TodoItems/... 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItemDto todoItem, CancellationToken token)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }
            await _todoListService.UpdateTodoItemAsync(id, todoItem.Description, todoItem.IsCompleted,token);

            return Ok();
        } 

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostTodoItem(TodoItemDto todoItem, CancellationToken token)
        {
            if (string.IsNullOrEmpty(todoItem?.Description))
            {
                return BadRequest("Description is required");
            }

            await _todoListService.AddTodoItemAsync(todoItem.Id, todoItem.Description,todoItem.IsCompleted, token);
             
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        } 
    }
}
