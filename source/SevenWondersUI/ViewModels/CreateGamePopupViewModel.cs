using System.Windows.Input;

namespace SevenWondersUI.ViewModels
{
    public class CreateGamePopupViewModel : BaseViewModel
    {
        private string m_roomName = string.Empty;

        public string RoomNameLabelText => "Szoba neve:";
        public string CreateButtonText => "Létrehozás";
        public string BackButtonText => "Vissza";

        public string RoomName
        {
            get => m_roomName;
            set
            {
                m_roomName = value;
                OnPropertyChanged();
                ((Command)CreateCommand).ChangeCanExecute();
            }
        }

        public bool CreateActivated { get; private set; }

        public ICommand CreateCommand { get; }
        public ICommand BackCommand { get; }

        public CreateGamePopupViewModel()
        {
            CreateCommand = new Command(OnCreate, CanCreate);
            BackCommand = new Command(OnBack);
        }

        public void Reset()
        {
            RoomName = string.Empty;
            CreateActivated = false;
        }

        private bool CanCreate() => !string.IsNullOrWhiteSpace(m_roomName) && m_roomName.Length >= 3;

        private void OnCreate()
        {
            CreateActivated = true;
        }

        private void OnBack()
        {
            CreateActivated = false;
        }
    }
}
