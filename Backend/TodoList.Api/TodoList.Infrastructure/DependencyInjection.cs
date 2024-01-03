

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Application.Interfaces;
using TodoList.Infrastructure.Persisitance;

namespace TodoList.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoItemsDB"));
            services.AddScoped<ITodoListRepository, TodoListRepository>();
            return services;
        }
    }
}
