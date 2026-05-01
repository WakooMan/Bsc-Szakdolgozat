namespace SevenWonders.UI.ViewModels
{
    public class LobbyMemberModel : BaseViewModel
    {
        private string m_userName = string.Empty;
        private bool m_isHost;
        private bool m_isLocalPlayer;

        public string UserName
        {
            get => m_userName;
            set
            {
                if (m_userName != value)
                {
                    m_userName = value;
                    OnPropertyChanged();
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
                }
            }
        }

        public bool IsLocalPlayer
        {
            get => m_isLocalPlayer;
            set
            {
                if (m_isLocalPlayer != value)
                {
                    m_isLocalPlayer = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
