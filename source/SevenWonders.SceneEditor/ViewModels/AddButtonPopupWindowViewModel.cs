namespace SevenWonders.SceneEditor.ViewModels
{
    public class AddButtonPopupWindowViewModel : AddPopupWindowViewModel
    {
        protected override bool CanExecuteAdd()
        {
            return base.CanExecuteAdd() && m_selectedTextureId > 0; // TODO: Check if it is a valid texture ID
        }

        public string ButtonText
        {
            get => m_buttonText;
            set
            {
                m_buttonText = value;
                OnPropertyChanged();
            }
        }

        public float FontSize
        {
            get => m_fontSize;
            set
            {
                m_fontSize = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => m_width;
            set
            {
                m_width = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public int Height
        {
            get => m_height;
            set
            {
                m_height = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public int SelectedTextureId
        {
            get => m_selectedTextureId;
            set
            {
                m_selectedTextureId = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public AddButtonPopupWindowViewModel() : base()
        {
            m_buttonText = string.Empty;
            m_fontSize = 24f;
            m_selectedTextureId = -1;
        }

        public override void Clear()
        {
            base.Clear();
            m_buttonText = string.Empty;
            m_fontSize = 24f;
            m_width = 0;
            m_height = 0;
            m_selectedTextureId = -1;
        }

        private string m_buttonText;
        private float m_fontSize;
        private int m_width;
        private int m_height;
        private int m_selectedTextureId;
    }
}
