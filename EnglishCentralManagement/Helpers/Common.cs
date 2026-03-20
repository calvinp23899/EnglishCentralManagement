using System.ComponentModel;
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

        public static string GetDescriptionEnum(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString();
        }
    }
}
