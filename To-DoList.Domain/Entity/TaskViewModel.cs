using System.ComponentModel.DataAnnotations;
using To_DoList.Domain.Enum;

namespace To_DoList.Domain.Entity;

public class TaskViewModel
{
    public long Id { get; set; }
    
    [Display(Name = "Название")]
    public string Name { get; set; }
    
    [Display(Name = "Статус")]
    public string IsDone { get; set; }
    
    [Display(Name = "Описание")]
    public string Description { get; set; }
    
    [Display(Name = "Приоритет")]
    public string Priority { get; set; }
    
    [Display(Name = "Дата создания")]
    public string CreatedAt { get; set; }
}