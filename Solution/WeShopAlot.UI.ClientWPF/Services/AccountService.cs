using System.Net;
using WeShopAlot.UI.ClientWPF.Models;

namespace WeShopAlot.UI.ClientWPF.Services
{
    /// <summary>Sign-in state, like Angular's AccountService (currentUser$).</summary>
    public class AccountService
    {
        private readonly ApiClient api;
        private readonly SessionStore session;

        public AccountService(ApiClient api, SessionStore session)
        {
            this.api = api;
            this.session = session;
        }

        public event EventHandler? Changed;

        public User? CurrentUser { get; private set; }

        public bool IsSignedIn => CurrentUser is not null;

        /// <summary>At startup: if a token was saved, ask the API who it belongs to (and get a fresh token).</summary>
        public async Task LoadCurrentUserAsync()
        {
            if (string.IsNullOrEmpty(session.Token)) return;
            api.Token = session.Token;
            try
            {
                var user = await api.GetCurrentUserAsync();
                if (user is null) SignOut();
                else SetUser(user);
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                SignOut(); // the saved token has expired
            }
        }

        public async Task LoginAsync(string email, string password)
        {
            var user = await api.LoginAsync(new LoginRequest(email, password))
                ?? throw new InvalidOperationException("The API did not return a user.");
            SetUser(user);
        }

        public async Task RegisterAsync(string displayName, string email, string password)
        {
            var user = await api.RegisterAsync(new RegisterRequest(displayName, email, password))
                ?? throw new InvalidOperationException("The API did not return a user.");
            SetUser(user);
        }

        public void SignOut()
        {
            CurrentUser = null;
            api.Token = null;
            session.Token = null;
            session.Save();
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public Task<bool> EmailExistsAsync(string email) => api.EmailExistsAsync(email);

        public Task<Address?> GetAddressAsync() => api.GetAddressAsync();

        public Task<Address?> UpdateAddressAsync(Address address) => api.UpdateAddressAsync(address);

        private void SetUser(User user)
        {
            CurrentUser = user;
            api.Token = user.Token;
            session.Token = user.Token;
            session.Save();
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
