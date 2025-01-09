namespace To_DoList.Domain.Interfaces.Services;

public interface ITaskExportService
{
    public Task<MemoryStream> ExportTasksToExcelAsync();
}