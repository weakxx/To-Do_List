using System.ComponentModel.DataAnnotations;

namespace To_DoList.Domain.Enum;

/// <summary>
/// Приоритет задачи
/// </summary>
public enum Priority
{
    [Display(Name = "Простая")]
    Easy = 0,
    [Display(Name = "Важная")]
    Medium = 1,
    [Display(Name = "Критичная")]
    High = 2
}