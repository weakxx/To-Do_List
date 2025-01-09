using Microsoft.AspNetCore.Mvc;
using To_DoList.Application.Services;
using To_DoList.Domain.Filters.Task;
using To_DoList.Domain.Interfaces.Services;
using To_DoList.Domain.ViewModels;

namespace To_DoList.Controllers;

public class TaskController : Controller
{
    private readonly ITaskService _taskService;
    private readonly ITaskExportService _taskExportService;

    public TaskController(ITaskService taskService, ITaskExportService taskExportService)
    {
        _taskService = taskService;
        _taskExportService = taskExportService;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpGet]
    public async Task<IActionResult> GetTask(long id)
    {
        var response = await _taskService.GetTask(id);
        return PartialView(response.Data);
    }
    
    [HttpPost]
    public async Task<IActionResult> EndTask(long id)
    {
        var response = await _taskService.EndTask(id);
        if (response.StatusCode == Domain.Enum.StatusCode.Ok)
        {
            return Ok(new { description = response.Description });
        }
        return BadRequest(new { description = response.Description });
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskViewModel model)
    {
        var response = await _taskService.CreateTaskAsync(model);
        if (response.StatusCode == Domain.Enum.StatusCode.Ok)
        {
            return Ok(new { description = response.Description });
        }
        return BadRequest(new { description = response.Description });
    }

    [HttpGet]
    public async Task<IActionResult> GetCompletedTasks()
    {
        var result = await _taskService.GetCompletedTasks();
        return Json(new { data = result.Data });
    }
    
    [HttpPost]
    public async Task<IActionResult> TaskHandler(TaskFilter filter)
    {
        var result = await _taskService.GetTasks(filter);
        return Json(new { data = result.Data });
    }


    [HttpGet]
    public async Task<IActionResult>ExportTasks()
    {
        var excelStream = await _taskExportService.ExportTasksToExcelAsync();
        return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Tasks.xlsx");
    }
}