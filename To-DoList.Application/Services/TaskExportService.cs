using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using To_DoList.Domain.Entity;
using To_DoList.Domain.Interfaces;
using To_DoList.Domain.Interfaces.Services;

namespace To_DoList.Application.Services;

public class TaskExportService : ITaskExportService
{
    private readonly IBaseRepository<TaskEntity> _taskRepository;

    public TaskExportService(IBaseRepository<TaskEntity> taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<MemoryStream> ExportTasksToExcelAsync()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        var tasks = await _taskRepository.GetAll().ToListAsync(); 
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Tasks"); 
        
        worksheet.Cells[1, 1].Value = "Id";
        worksheet.Cells[1, 2].Value = "Name";
        worksheet.Cells[1, 3].Value = "Description";
        worksheet.Cells[1, 4].Value = "Priority";
        worksheet.Cells[1, 5].Value = "Created At";
        worksheet.Cells[1, 6].Value = "Is Done";

        for (int i = 0; i < tasks.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = tasks[i].Id;
            worksheet.Cells[i + 2, 2].Value = tasks[i].Name;
            worksheet.Cells[i + 2, 3].Value = tasks[i].Description;
            worksheet.Cells[i + 2, 4].Value = tasks[i].Priority;
            worksheet.Cells[i + 2, 5].Value = tasks[i].CreatedAt;
            worksheet.Cells[i + 2, 6].Value = tasks[i].IsDone ? "Yes" : "No";
        }

        worksheet.Cells.AutoFitColumns();

        var memoryStream = new MemoryStream();
        await package.SaveAsAsync(memoryStream);
        memoryStream.Position = 0;
        var content = memoryStream.ToArray();

        return memoryStream;
    }
}