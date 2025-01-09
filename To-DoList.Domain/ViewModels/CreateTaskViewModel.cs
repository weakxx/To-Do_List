using To_DoList.Domain.Enum;

namespace To_DoList.Domain.ViewModels;

/// <summary>
/// Данная модель содержит свойства, которые мы заполняем в форме 
/// </summary>
public class CreateTaskViewModel
{
    public string Name { get; set; }
    
    public Priority Priority { get; set; }
    
    public string Description { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentNullException(Name, "Укажите название задачи");
        }
        
        if (string.IsNullOrWhiteSpace(Description))
        {
            throw new ArgumentNullException(Description, "Укажите описание задачи");
        }
    }
}