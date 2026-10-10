using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>Called when the view model is shown, so it can fetch its data.</summary>
        public virtual Task LoadAsync() => Task.CompletedTask;

        /// <summary>Shown in the page band under the header; null hides the band (Home), as in the Angular client.</summary>
        public virtual string? PageTitle => null;

        /// <summary>The trail shown at the right of the page band, e.g. "Home / Shop".</summary>
        public virtual string Breadcrumb => PageTitle is null ? "" : $"Home  /  {PageTitle}";

        protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
