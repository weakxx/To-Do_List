using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace To_DoList.Domain.Extensions;

public static class EnumExtension
{
    public static string GetDisplayName(this System.Enum enumValue)
    {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttributes<DisplayAttribute>().FirstOrDefault()
                ?.GetName() ?? "Неопределенный";
    }
}