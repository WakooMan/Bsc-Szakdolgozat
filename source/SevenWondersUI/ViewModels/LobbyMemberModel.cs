namespace SevenWondersUI.ViewModels
{
    public class LobbyMemberModel : BaseViewModel
    {
        private string m_userName = string.Empty;
        private bool m_isHost;

        public string UserName
        {
            get => m_userName;
            set
            {
                if (m_userName != value)
                {
                    m_userName = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }

        public bool IsHost
        {
            get => m_isHost;
            set
            {
                if (m_isHost != value)
                {
                    m_isHost = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }

        public string DisplayName => IsHost ? $"{UserName} ??" : UserName;
    }
}
