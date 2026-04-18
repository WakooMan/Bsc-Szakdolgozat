using SevenWonders.WebClient.Model.Services;
using SevenWondersUI.Services;
using System.Windows.Input;
using WebServer.Contract;

namespace SevenWondersUI.ViewModels
{
    public class LoginPageViewModel: BaseViewModel
    {
        public LoginPageViewModel(INavigationService navigationService, IAuthService authService)
        {
            m_navigationService = navigationService;
            m_authService = authService;
            m_userNameEntry = ("Felhasználónév:", "Wakoo");
            m_passwordEntry = ("Jelszó:", "NagyonErosJelszo123!");
            m_loginText = "Belépés";
            m_backText = "Vissza";
            m_loginCommand = new Command(OnLogin, ValidateTexts);
            BackCommand = new Command(OnBack, () => true);
            NavigateToRegisterCommand = new Command(OnNavigateToRegister);
        }

        public string UserNameText
        {
            get
            {
                return m_userNameEntry.labelText;
            }
            set
            {
                if (m_userNameEntry.labelText != value)
                {
                    m_userNameEntry.labelText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string UserName
        {
            get
            {
                return m_userNameEntry.entryText;
            }
            set
            {
                if (m_userNameEntry.entryText != value)
                {
                    m_userNameEntry.entryText = value;
                    OnPropertyChanged();
                    m_loginCommand.ChangeCanExecute();
                }
            }
        }

        public string PasswordText
        {
            get
            {
                return m_passwordEntry.labelText;
            }
            set
            {
                if (m_passwordEntry.labelText != value)
                {
                    m_passwordEntry.labelText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get
            {
                return m_passwordEntry.entryText;
            }
            set
            {
                if (m_passwordEntry.entryText != value)
                {
                    m_passwordEntry.entryText = value;
                    OnPropertyChanged();
                    m_loginCommand.ChangeCanExecute();
                }
            }
        }

        public string LoginText
        {
            get
            {
                return m_loginText;
            }
            set
            {
                if (m_loginText != value)
                {
                    m_loginText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BackText
        {
            get
            {
                return m_backText;
            }
            set
            {
                if (m_backText != value)
                {
                    m_backText = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoginCommand => m_loginCommand;
        public ICommand BackCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        private bool ValidateTexts()
        {
            return ValidateTexts(m_userNameEntry.entryText) && ValidateTexts(m_passwordEntry.entryText);
        }

        private bool ValidateTexts(string text)
        {
            return !string.IsNullOrEmpty(text) && text.Length >= 2;
        }

        private async void OnLogin()
        {
            LoginResponse? result = await m_authService.LoginAsync(m_userNameEntry.entryText, m_passwordEntry.entryText);
            if (result is not null && result.Success)
            {
                await m_navigationService.NavigateToAsync("//ConnectPage", new Dictionary<string, object>
                {
                    { "AuthToken", result.Token },
                    { "UserName", m_userNameEntry.entryText }
                });
                m_userNameEntry.entryText = string.Empty;
                m_passwordEntry.entryText = string.Empty;
            }
            else
            {
                await Shell.Current.DisplayAlert("Sikertelen belépés", result?.Message ?? "Felhasználónév vagy jelszó nem megfelelő!", "OK");
            }
        }

        private async void OnBack()
        {
            await m_navigationService.NavigateToAsync("//MainPage");
            m_userNameEntry.entryText = string.Empty;
            m_passwordEntry.entryText = string.Empty;
        }

        private async void OnNavigateToRegister()
        {
            await m_navigationService.NavigateToAsync("//RegisterPage");
        }


        private (string labelText, string entryText) m_userNameEntry;
        private (string labelText, string entryText) m_passwordEntry;
        private string m_loginText;
        private string m_backText;
        private readonly INavigationService m_navigationService;
        private readonly IAuthService m_authService;
        private readonly Command m_loginCommand;
    }
}
