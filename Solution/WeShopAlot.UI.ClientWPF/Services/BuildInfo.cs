using System.Globalization;
using System.Reflection;

namespace WeShopAlot.UI.ClientWPF.Services
{
    /// <summary>
    /// When this build was made: the project file stamps the time into the assembly
    /// ([AssemblyMetadata("BuildTime", ...)]), the WPF version of Angular's BUILD_TIME define.
    /// </summary>
    public static class BuildInfo
    {
        public static DateTime? BuildTime { get; } = Read();

        public static string Text => BuildTime is { } time ? $"Built {time.ToLocalTime():g}" : "Development build";

        private static DateTime? Read()
        {
            var value = typeof(BuildInfo).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
                .FirstOrDefault(a => a.Key == "BuildTime")?.Value;
            return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var time)
                ? time
                : null;
        }
    }
}
