using System.Net;
using System.Net.Http;
using WeShopAlot.UI.ClientWPF.Command;
using WeShopAlot.UI.ClientWPF.Services;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>Sign in (Angular's LoginComponent): POST /api/account/login, keep the returned token.</summary>
    public class LoginViewModel : ValidatingViewModelBase
    {
        private readonly AccountService account;
        private readonly Action afterSignIn;

        private string email = "";
        private string password = "";
        private bool isBusy;
        private string? errorMessage;

        public LoginViewModel(AccountService account, IShell shell, Action afterSignIn)
        {
            this.account = account;
            this.afterSignIn = afterSignIn;
            LoginCommand = new DelegateCommand(async _ => await LoginAsync(), _ => !isBusy);
            RegisterCommand = new DelegateCommand(_ => shell.GoRegister(afterSignIn));
        }

        public DelegateCommand LoginCommand { get; }
        public DelegateCommand RegisterCommand { get; }

        public string Email
        {
            get => email;
            set
            {
                if (SetProperty(ref email, value ?? "")) ValidateEmail();
            }
        }

        /// <summary>Set from the PasswordBox (WPF does not allow binding to a password).</summary>
        public string Password
        {
            get => password;
            set => password = value ?? "";
        }

        public string? ErrorMessage
        {
            get => errorMessage;
            private set => SetProperty(ref errorMessage, value);
        }

        private bool ValidateEmail()
        {
            SetError(nameof(Email), FormRules.Email(Email));
            return !HasErrors;
        }

        private async Task LoginAsync()
        {
            ErrorMessage = null;
            if (!ValidateEmail()) return;
            if (string.IsNullOrEmpty(Password)) { ErrorMessage = "Please enter your password."; return; }

            isBusy = true;
            LoginCommand.RaiseCanExecuteChanged();
            try
            {
                await account.LoginAsync(Email.Trim(), Password);
                afterSignIn();
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                ErrorMessage = "That email and password don't match an account.";
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                ErrorMessage = ex is ApiException apiError ? apiError.Details : ex.Message;
            }
            finally
            {
                isBusy = false;
                LoginCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
