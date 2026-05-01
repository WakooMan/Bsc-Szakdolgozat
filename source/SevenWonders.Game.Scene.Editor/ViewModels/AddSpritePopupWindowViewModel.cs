namespace SevenWonders.Game.Scene.Editor.ViewModels
{
    public class AddSpritePopupWindowViewModel: AddPopupWindowViewModel
    {
        public int FrameWidth
        {
            get
            {
                return m_frameWidth;
            }
            set
            {
                m_frameWidth = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public int FrameHeight
        {
            get
            {
                return m_frameHeight;
            }
            set
            {
                m_frameHeight = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public int Rows
        {
            get
            {
                return m_rows;
            }
            set
            {
                m_rows = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public int Columns
        {
            get
            {
                return m_columns;
            }
            set
            {
                m_columns = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public int TextureId
        {
            get
            {
                return m_textureId;
            }
            set
            {
                m_textureId = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public AddSpritePopupWindowViewModel() : base()
        {
            m_textureId = -1;
        }

        protected override bool CanExecuteAdd()
        {
            return base.CanExecuteAdd() && m_textureId > 0; // TODO: Check if it is a valid texture ID
        }

        private int m_frameWidth;
        private int m_frameHeight;
        private int m_rows;
        private int m_columns;
        private int m_textureId;
        
    }
}
