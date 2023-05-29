using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace WeShopAlot.Shared.Extensions
{
    public static class StringExtension
    {
        public static Dictionary<string, string> DecodeReturnedData(this object value)
        {
            if (value == null)
                return null;
            string info = System.Net.WebUtility.UrlDecode(value.ToString());
            var result = info.Split(';')
                .Select(x => x.Split(':'))
                .ToDictionary(x => x[0], x => x[1]);
            return result;
        }

        /// <summary>
        /// Get the Description of the Enum/Model attribute created. It works with the following data annotation attributes in class or Enum
        ///       [Description("Late Fee")] or [DisplayName("Processing Fee")] or [Display(Description =EnumDbValues._lateFee)]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this object value)
        {
            string displayName = string.Empty;
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field == null)
            {
                MemberInfo property = value as MemberInfo;
                DescriptionAttribute descAttribute = Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (descAttribute != null)
                {
                    displayName = descAttribute.Description;
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
                            displayName = displayAttribute.Description == null ? displayAttribute.Name : displayAttribute.Description;
                        }
                    }
                }

                return string.IsNullOrEmpty(displayName) ? value.ToString() : displayName;
            }
            else
            {
                DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                displayName = attribute == null ? value.ToString() : attribute.Description;
            }
            return displayName;

        }

        public static string EmptyIfNull(this object value)
        {
            if (value == null)
                return "";
            return value.ToString();
        }

        /// <summary>
        /// Mask any string with special character 
        /// i.e. ("jonathan.li@com.state.oh.us".MaskWith(0, 5,'*')); jonat**********************
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string MaskWith(this string input, int startIndex, int length, char character)
        {
            return (input?.Length >= startIndex + length) ?
                 string.Concat(Enumerable.Repeat(character, startIndex)) +
                 input.Substring(startIndex, length) +
                 string.Concat(Enumerable.Repeat(character, input.Length - (startIndex + length)))
                 : "";
        }

        public static Guid ToGuid(this object value)
        {
            if (value == null)
                return Guid.Empty;

            if (Guid.TryParse(value.ToString(), out var newGuid))
            {
                return newGuid;
            }
            else
            {
                return Guid.Empty;
            }
        }

        public static string WithoutSpaces(this string value)
        {
            var sb = new StringBuilder();
            foreach (var c in value)
            {
                if (c != ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string HighLight(this string searchString, string highlightString)
        {
            return searchString.Replace(highlightString, $"<mark>{highlightString}</mark>", true, null);
        }
    }
}
