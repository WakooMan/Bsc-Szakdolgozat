using SevenWonders.WebClient.Model.Services;
using SevenWondersUI.Services;
using System.Text.RegularExpressions;
using System.Windows.Input;
using WebServer.Contract;

namespace SevenWondersUI.ViewModels
{
    public class RegisterPageViewModel : BaseViewModel
    {
        public RegisterPageViewModel(INavigationService navigationService, IAuthService authService)
        {
            m_navigationService = navigationService;
            m_authService = authService;
            m_userName = string.Empty;
            m_email = string.Empty;
            m_password = string.Empty;
            m_userNameError = string.Empty;
            m_emailError = string.Empty;
            m_passwordError = string.Empty;
            m_registerCommand = new Command(OnRegister, CanRegister);
            NavigateToLoginCommand = new Command(OnNavigateToLogin);
        }

        public string UserName
        {
            get => m_userName;
            set
            {
                if (m_userName != value)
                {
                    m_userName = value;
                    OnPropertyChanged();
                    ValidateUserName();
                    m_registerCommand.ChangeCanExecute();
                }
            }
        }

        public string Email
        {
            get => m_email;
            set
            {
                if (m_email != value)
                {
                    m_email = value;
                    OnPropertyChanged();
                    ValidateEmail();
                    m_registerCommand.ChangeCanExecute();
                }
            }
        }

        public string Password
        {
            get => m_password;
            set
            {
                if (m_password != value)
                {
                    m_password = value;
                    OnPropertyChanged();
                    ValidatePassword();
                    m_registerCommand.ChangeCanExecute();
                }
            }
        }

        public string UserNameError
        {
            get => m_userNameError;
            set
            {
                if (m_userNameError != value)
                {
                    m_userNameError = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EmailError
        {
            get => m_emailError;
            set
            {
                if (m_emailError != value)
                {
                    m_emailError = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PasswordError
        {
            get => m_passwordError;
            set
            {
                if (m_passwordError != value)
                {
                    m_passwordError = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand RegisterCommand => m_registerCommand;
        public ICommand NavigateToLoginCommand { get; }

        private void ValidateUserName()
        {
            if (string.IsNullOrWhiteSpace(m_userName))
                UserNameError = "A felhasználónév megadása kötelez?.";
            else if (m_userName.Length < 3)
                UserNameError = "A felhasználónév legalább 3 karakter legyen.";
            else
                UserNameError = string.Empty;
        }

        private void ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(m_email))
                EmailError = "Az e-mail cím megadása kötelez?.";
            else if (!Regex.IsMatch(m_email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                EmailError = "Érvénytelen e-mail cím.";
            else
                EmailError = string.Empty;
        }

        private void ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(m_password))
                PasswordError = "A jelszó megadása kötelez?.";
            else if (m_password.Length < 6)
                PasswordError = "A jelszó legalább 6 karakter legyen.";
            else if (!Regex.IsMatch(m_password, @"[A-Z]"))
                PasswordError = "A jelszónak tartalmaznia kell nagybet?t.";
            else if (!Regex.IsMatch(m_password, @"[a-z]"))
                PasswordError = "A jelszónak tartalmaznia kell kisbet?t.";
            else if (!Regex.IsMatch(m_password, @"\d"))
                PasswordError = "A jelszónak tartalmaznia kell számot.";
            else if (!Regex.IsMatch(m_password, @"[\W_]"))
                PasswordError = "A jelszónak tartalmaznia kell speciális karaktert.";
            else
                PasswordError = string.Empty;
        }

        private bool CanRegister()
        {
            ValidateUserName();
            ValidateEmail();
            ValidatePassword();
            return string.IsNullOrEmpty(m_userNameError)
                && string.IsNullOrEmpty(m_emailError)
                && string.IsNullOrEmpty(m_passwordError)
                && !string.IsNullOrWhiteSpace(m_userName)
                && !string.IsNullOrWhiteSpace(m_email)
                && !string.IsNullOrWhiteSpace(m_password);
        }

        private async void OnRegister()
        {
            RegisterResponse? result = await m_authService.RegisterAsync(m_userName, m_email, m_password);
            if (result is not null && result.Success)
            {
                await Shell.Current.DisplayAlert("Sikeres regisztráció", "A regisztráció sikeres volt!", "OK");
                await m_navigationService.NavigateToAsync("//LoginPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Sikertelen regisztráció", result?.Message ?? "A regisztráció nem sikerült.", "OK");
            }
        }

        private async void OnNavigateToLogin()
        {
            await m_navigationService.NavigateToAsync("//LoginPage");
        }

        private string m_userName;
        private string m_email;
        private string m_password;
        private string m_userNameError;
        private string m_emailError;
        private string m_passwordError;
        private readonly INavigationService m_navigationService;
        private readonly IAuthService m_authService;
        private readonly Command m_registerCommand;
    }
}
