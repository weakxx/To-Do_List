using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using To_DoList.Application.Services;
using To_DoList.DAL.Repositories;
using To_DoList.Domain.Entity;
using To_DoList.Domain.Interfaces;
using To_DoList.Domain.Interfaces.Services;

namespace To_DoList.DAL.DependencyInjection;

public static class DependencyInjection
{
    public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgresSQL");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        services.InitRepositories();
    }

    private static void InitRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<TaskEntity>, TaskRepository>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ITaskExportService, TaskExportService>();
    }
}