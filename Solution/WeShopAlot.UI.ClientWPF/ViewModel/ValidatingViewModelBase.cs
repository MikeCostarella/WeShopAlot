using System.Collections;
using System.ComponentModel;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>
    /// Field-level validation for forms (the desktop version of Angular's reactive-form validators).
    /// WPF reads the errors through INotifyDataErrorInfo, and the TextBox style in Resources/Styles.xaml
    /// shows them in red under the field.
    /// </summary>
    public abstract class ValidatingViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> errors = new();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => errors.Count > 0;

        public IEnumerable GetErrors(string? propertyName) =>
            propertyName is not null && errors.TryGetValue(propertyName, out var list)
                ? list
                : Array.Empty<string>();

        /// <summary>Sets (or, with null, clears) the single error shown for a field.</summary>
        protected void SetError(string propertyName, string? error)
        {
            var had = errors.ContainsKey(propertyName);
            if (error is null)
            {
                if (!had) return;
                errors.Remove(propertyName);
            }
            else
            {
                if (had && errors[propertyName].Count == 1 && errors[propertyName][0] == error) return;
                errors[propertyName] = new List<string> { error };
            }
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged(nameof(HasErrors));
        }

        protected void ClearAllErrors()
        {
            foreach (var name in errors.Keys.ToList()) SetError(name, null);
        }
    }
}
