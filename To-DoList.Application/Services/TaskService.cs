using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using To_DoList.Domain.Entity;
using To_DoList.Domain.Enum;
using To_DoList.Domain.Extensions;
using To_DoList.Domain.Filters.Task;
using To_DoList.Domain.Interfaces;
using To_DoList.Domain.Interfaces.Services;
using To_DoList.Domain.Response;
using To_DoList.Domain.ViewModels;

namespace To_DoList.Application.Services;

public class TaskService : ITaskService
{
    private readonly ILogger<TaskService> _logger;
    private readonly IBaseRepository<TaskEntity> _taskRepository;

    public TaskService(IBaseRepository<TaskEntity> taskRepository, ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task<IBaseResponse<bool>> EndTask(long id)
    {
        try
        {
            var task = await _taskRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return new BaseResponse<bool>()
                {
                    Description = "Задача не найдена",
                    StatusCode = StatusCode.TaskNotFound
                };
            }

            task.IsDone = true;
            await _taskRepository.Update(task);

            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.Ok,
                Description = "Задача выполнена"
            };
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.EndTask]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = "Внутренняя ошибка",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<TaskViewModel>> GetTask(long id)
    {
        try
        {
            var task = await _taskRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return new BaseResponse<TaskViewModel>()
                {
                    Description = "Задача не найдена",
                    StatusCode = StatusCode.TaskNotFound
                };
            }

            var data = new TaskViewModel()
            {
                Id = task.Id,
                Name = task.Name,
                IsDone = task.IsDone == true ? "Готово" : "Не готово",
                Description = task.Description,
                Priority = task.Priority.GetDisplayName(),
                CreatedAt = task.CreatedAt.ToLongDateString()
            };

            return new BaseResponse<TaskViewModel>()
            {
                StatusCode = StatusCode.Ok,
                Data = data
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.GetTask]: {ex.Message}");
            return new BaseResponse<TaskViewModel>()
            {
                Description = "Внутренняя ошибка",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetCompletedTasks()
    {
        try
        {
            var tasks = await _taskRepository.GetAll()
                .Where(x => x.IsDone)
                .Select(x => new TaskViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsDone = x.IsDone == true ? "Готово" : "Не готово",
                    Description = x.Description,
                    Priority = x.Priority.GetDisplayName(),
                    CreatedAt = x.CreatedAt.ToLongDateString()
                }).ToListAsync();
            
            _logger.LogInformation($"[TaskService.GetCompletedTasks] получено элементов: {tasks.Count()}");
            return new BaseResponse<IEnumerable<TaskViewModel>>()
            {
                Data = tasks,
                StatusCode = StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.GetCompletedTasks]: {ex.Message}");
            return new BaseResponse<IEnumerable<TaskViewModel>>()
            {
                Description = "Внутренняя ошибка",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetTasks(TaskFilter filter)
    {
        try
        {
            var tasks = _taskRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name == filter.Name)
                .WhereIf(filter.Priority.HasValue, x => x.Priority == filter.Priority)
                .Where(x => !x.IsDone) 
                .Select(x => new TaskViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsDone = x.IsDone == true ? "Готово" : "Не готово",
                    Description = x.Description,
                    Priority = x.Priority.GetDisplayName(),
                    CreatedAt = x.CreatedAt.ToLongDateString()
                });

            _logger.LogInformation($"[TaskService.GetTasks] получено элементов: {tasks.Count()}");
            return new BaseResponse<IEnumerable<TaskViewModel>>()
            {
                Data = tasks,
                StatusCode = StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.GetTasks]: {ex.Message}");
            return new BaseResponse<IEnumerable<TaskViewModel>>()
            {
                Description = "Внутренняя ошибка",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<TaskEntity>> CreateTaskAsync(CreateTaskViewModel model)
    {
        try
        {
            model.Validate();
            
            _logger.LogInformation($"Запрос на создание задачи - {model.Name}");
            
            var task = await _taskRepository.GetAll()
                .Where(x => x.CreatedAt.Date == DateTime.UtcNow.Date) 
                .FirstOrDefaultAsync(x => x.Name == model.Name);

            if (task != null)
            {
                return new BaseResponse<TaskEntity>()
                {
                    Description = "Задача с таким названием уже есть",
                    StatusCode = StatusCode.TaskIsHasAlready
                };
            }

            task = new TaskEntity()
            {
                Name = model.Name,
                Description = model.Description,
                Priority = model.Priority,
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
            await _taskRepository.CreateAsync(task);
            
            _logger.LogInformation($"Задача {task.Name} добавлена. {task.CreatedAt}");
            return new BaseResponse<TaskEntity>()
            {
                Description = "Задача создана",
                StatusCode = StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
            return new BaseResponse<TaskEntity>()
            {
                Description = $"{ex.Message}",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
}