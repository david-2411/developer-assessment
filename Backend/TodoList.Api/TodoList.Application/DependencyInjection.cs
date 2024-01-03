using Microsoft.Extensions.DependencyInjection;
using TodoList.Application.Services.TodoList;

namespace TodoList.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITodoListService, TodoListService>();
            return services;
        }
    }
}
