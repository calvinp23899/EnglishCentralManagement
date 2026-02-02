using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EnglishCentralManagement.Helpers
{
    public static class Common
    {
        public static string GetDisplayEnumName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute = field?
                .GetCustomAttribute<DisplayAttribute>();

            return attribute?.Name ?? value.ToString();
        }
    }
}
