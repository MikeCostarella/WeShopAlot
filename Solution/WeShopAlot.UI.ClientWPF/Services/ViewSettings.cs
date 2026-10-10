using System.ComponentModel;

namespace WeShopAlot.UI.ClientWPF.Services
{
    /// <summary>
    /// Cards or List on the shop page. Shared by the shop page and the hamburger menu's View section,
    /// and remembered between runs (session.json), like the Angular client's ViewModeService.
    /// </summary>
    public class ViewSettings : INotifyPropertyChanged
    {
        private readonly SessionStore session;

        public ViewSettings(SessionStore session)
        {
            this.session = session;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool ShowCards
        {
            get => session.ShopView != "list";
            set
            {
                if (value == ShowCards) return;
                session.ShopView = value ? "cards" : "list";
                session.Save();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowCards)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowList)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModeName)));
            }
        }

        public bool ShowList
        {
            get => !ShowCards;
            set => ShowCards = !value;
        }

        public string ModeName => ShowCards ? "Cards" : "List";
    }
}
