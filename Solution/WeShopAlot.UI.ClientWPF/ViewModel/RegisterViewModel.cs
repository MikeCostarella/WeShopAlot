using System.Net.Http;
using WeShopAlot.UI.ClientWPF.Command;
using WeShopAlot.UI.ClientWPF.Services;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>
    /// Create an account (Angular's RegisterComponent): the same rules as the Angular form and RegisterDto,
    /// including the "email already taken" check against GET /api/account/emailexists.
    /// </summary>
    public class RegisterViewModel : ValidatingViewModelBase
    {
        private readonly AccountService account;
        private readonly Action afterSignIn;

        private string displayName = "";
        private string email = "";
        private string password = "";
        private bool isBusy;
        private string? errorMessage;
        private int emailCheckVersion;

        public RegisterViewModel(AccountService account, IShell shell, Action afterSignIn)
        {
            this.account = account;
            this.afterSignIn = afterSignIn;
            RegisterCommand = new DelegateCommand(async _ => await RegisterAsync(), _ => !isBusy);
            LoginCommand = new DelegateCommand(_ => shell.GoLogin(afterSignIn));
        }

        public DelegateCommand RegisterCommand { get; }
        public DelegateCommand LoginCommand { get; }

        public string DisplayName
        {
            get => displayName;
            set
            {
                if (SetProperty(ref displayName, value ?? "")) ValidateDisplayName();
            }
        }

        public string Email
        {
            get => email;
            set
            {
                if (SetProperty(ref email, value ?? "")) _ = ValidateEmailAsync();
            }
        }

        /// <summary>Set from the PasswordBox (WPF does not allow binding to a password).</summary>
        public string Password
        {
            get => password;
            set
            {
                password = value ?? "";
                PasswordError = FormRules.Password(password);
            }
        }

        /// <summary>Shown under the PasswordBox (which has no validation template of its own).</summary>
        public string? PasswordError
        {
            get => passwordError;
            private set => SetProperty(ref passwordError, value);
        }
        private string? passwordError;

        public string? ErrorMessage
        {
            get => errorMessage;
            private set => SetProperty(ref errorMessage, value);
        }

        private void ValidateDisplayName() =>
            SetError(nameof(DisplayName), FormRules.Required(DisplayName, "Display name"));

        /// <summary>Checks the format at once, then (after a pause in typing, as Angular debounces) asks the API.</summary>
        private async Task ValidateEmailAsync()
        {
            var formatError = FormRules.Email(Email);
            SetError(nameof(Email), formatError);
            if (formatError is not null) return;

            var version = ++emailCheckVersion;
            await Task.Delay(TimeSpan.FromMilliseconds(600));
            if (version != emailCheckVersion) return; // still typing
            try
            {
                if (await account.EmailExistsAsync(Email.Trim()) && version == emailCheckVersion)
                    SetError(nameof(Email), "That email address is already taken.");
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                // The API will refuse a duplicate on submit anyway.
            }
        }

        private async Task RegisterAsync()
        {
            ErrorMessage = null;
            ValidateDisplayName();
            PasswordError = FormRules.Password(Password);
            if (FormRules.Email(Email) is string emailError) SetError(nameof(Email), emailError);
            if (HasErrors || PasswordError is not null) return;

            isBusy = true;
            RegisterCommand.RaiseCanExecuteChanged();
            try
            {
                await account.RegisterAsync(DisplayName.Trim(), Email.Trim(), Password);
                afterSignIn();
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                // e.g. the API's "Email address is in use" validation error
                ErrorMessage = ex is ApiException apiError ? apiError.Details : ex.Message;
            }
            finally
            {
                isBusy = false;
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
