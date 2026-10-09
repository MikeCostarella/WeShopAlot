using System.Text.RegularExpressions;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>
    /// The same rules as the Angular forms (Validators.*) and the API's DTO attributes, so the desktop client
    /// never sends something the API will reject. Each returns an error message, or null when the value is fine.
    /// </summary>
    public static class FormRules
    {
        // RegisterDto and register.component.ts: upper, lower, digit, one of !@#$&*, at least 8 characters.
        private static readonly Regex ComplexPassword = new("^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,}$");
        private static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        private static readonly Regex StatePattern = new("^[A-Za-z]{2}$");
        private static readonly Regex ZipPattern = new(@"^\d{5}(-\d{4})?$");

        public static string? Required(string? value, string label) =>
            string.IsNullOrWhiteSpace(value) ? $"{label} is required" : null;

        public static string? MaxLength(string? value, string label, int max) =>
            Required(value, label) ?? (value!.Trim().Length > max ? $"{label} can be at most {max} characters" : null);

        public static string? Email(string? value) =>
            Required(value, "Email") ?? (EmailPattern.IsMatch(value!.Trim()) ? null : "Enter a valid email address");

        public static string? Password(string? value) =>
            Required(value, "Password") ?? (ComplexPassword.IsMatch(value!) ? null
                : "Password needs 8+ characters with an uppercase letter, a lowercase letter, a number and one of !@#$&*");

        public static string? State(string? value) =>
            Required(value, "State") ?? (StatePattern.IsMatch(value!.Trim()) ? null : "State must be a 2-letter code, such as OH");

        public static string? Zipcode(string? value) =>
            Required(value, "Zip code") ?? (ZipPattern.IsMatch(value!.Trim()) ? null : "Zip code must be 5 digits, or ZIP+4");
    }
}
