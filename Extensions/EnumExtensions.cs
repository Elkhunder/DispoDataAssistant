using System;
using System.ComponentModel;
using System.Linq;

namespace DispoDataAssistant.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString()).FirstOrDefault();

            if (memberInfo == null)
            {
                return value.ToString();
            }

            DescriptionAttribute? descriptionAttribute = (DescriptionAttribute?)memberInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault();

            return descriptionAttribute?.Description ?? value.ToString();
        }
    }
}
