using To_DoList.Domain.Enum;

namespace To_DoList.Domain.Filters.Task;

public class TaskFilter
{
    public string Name { get; set; }
    
    public Priority? Priority { get; set; }
}