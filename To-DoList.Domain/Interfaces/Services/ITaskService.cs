using To_DoList.Domain.Entity;
using To_DoList.Domain.Filters.Task;
using To_DoList.Domain.Response;
using To_DoList.Domain.ViewModels;

namespace To_DoList.Domain.Interfaces.Services;

public interface ITaskService
{
    Task<IBaseResponse<bool>> EndTask(long id);
    
    Task<IBaseResponse<TaskViewModel>> GetTask(long id);

    Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetCompletedTasks();

    Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetTasks(TaskFilter filter);
    
    Task<IBaseResponse<TaskEntity>> CreateTaskAsync(CreateTaskViewModel model);
}