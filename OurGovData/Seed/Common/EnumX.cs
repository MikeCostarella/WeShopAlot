using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;

namespace MeShopAlot.Data.Seed.Common
{
    public static class EnumX
    {
        public static T GetValue<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                    else
                    {
                        if (field.Name == description)
                        {
                            return (T)field.GetValue(null);
                        }
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }
            return default(T);
        }

        public static T GetName<T>(string name) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (field.Name == name)
                {
                    return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", nameof(name));
        }
        
        public static ValueConverter Converter<T>() where T : Enum
        {
            var converter = new ValueConverter<T, string>(v => v.Description(), v => GetValue<T>(v));
            return converter;
        }

        public static ValueConverter NameConverter<T>() where T : Enum
        {
            var converter = new ValueConverter<T, string>(v => Enum.GetName(typeof(T), v), v => GetValue<T>(v));
            return converter;
        }

        public static string Description(this Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val?
                .GetType()?
                .GetField(val.ToString())?
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes?.Length > 0 ? attributes[0]?.Description : string.Empty;
        }
    }
}
