using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MeShopAlot.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string GetDescription(this object value)
        {
            string displayName = string.Empty;
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field == null) {
                MemberInfo property = value as MemberInfo;
                DescriptionAttribute descriptionAttribute = Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (descriptionAttribute != null)
                {
                    displayName = descriptionAttribute.Description;
                }
                else
                {
                    DisplayNameAttribute displayNameAttribute = Attribute.GetCustomAttribute(property, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                    if (displayNameAttribute != null)
                    {
                        displayName = displayNameAttribute.DisplayName;
                    }
                    else
                    {
                        DisplayAttribute displayAttribute = Attribute.GetCustomAttribute(property, typeof(DisplayAttribute)) as DisplayAttribute;
                        if (displayAttribute != null)
                        {
                            displayName = displayAttribute.Description == null? displayAttribute.Name : displayAttribute.Description;
                        }
                    }
                }
                return string.IsNullOrEmpty(displayName) ? string.Empty : displayName;
            }
            else
            {
                DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                displayName = attribute == null? value.ToString() : attribute.Description;
            }
            return displayName;
        }
    }
}