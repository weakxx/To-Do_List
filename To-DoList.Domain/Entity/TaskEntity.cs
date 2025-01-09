using To_DoList.Domain.Enum;

namespace To_DoList.Domain.Entity;

public class TaskEntity
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public bool IsDone { get; set; }
    
    public string Description { get; set; }
    
    public Priority Priority { get; set; }
    
    public DateTime CreatedAt { get; set; }
}