namespace SevenWonders.UI.ViewModels
{
    public class RoomModel: BaseViewModel
    {
        public RoomModel()
        {
            m_roomName = string.Empty;
            m_hostName = string.Empty;
            Code = string.Empty;
            m_backgroundColor = Colors.Black;
            m_textColor = Colors.White;
        }

        public string RoomName
        {
            get
            {
                return m_roomName;
            }
            set
            {
                if (m_roomName != value)
                {
                    m_roomName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string HostName
        {
            get
            {
                return m_hostName;
            }
            set
            {
                if (m_hostName != value)
                {
                    m_hostName = value;
                    OnPropertyChanged();
                }
            }
        }
        public Color BackgroundColor
        {
            get
            {
                return m_backgroundColor;
            }
            set
            {
                if (m_backgroundColor != value)
                {
                    m_backgroundColor = value;
                    OnPropertyChanged();
                }
            }
        }
        public Color TextColor
        {
            get
            {
                return m_textColor;
            }
            set
            {
                if (m_textColor != value)
                {
                    m_textColor = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Code { get; set; }

        public string DisplayName => $"{RoomName} (Host: {HostName})";

        private string m_hostName;
        private string m_roomName;
        private Color m_backgroundColor;
        private Color m_textColor;
    }
}
