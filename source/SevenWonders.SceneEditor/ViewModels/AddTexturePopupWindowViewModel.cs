namespace SevenWonders.SceneEditor.ViewModels
{
    public class AddTexturePopupWindowViewModel: AddPopupWindowViewModel
    {
        protected override bool CanExecuteAdd()
        {
            return base.CanExecuteAdd() && m_width > 0 && m_height > 0 && !string.IsNullOrEmpty(m_selectedFilePath);
        }

        public int Width
        {
            get
            {
                return m_width;
            }
            set
            {
                m_width = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public int Height
        {
            get
            {
                return m_height;
            }
            set
            {
                m_height = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public string SelectedFileName
        {
            get
            {
                return m_selectedFileName;
            }
            set
            {
                m_selectedFileName = value;
                OnPropertyChanged();
            }
        }

        public string SelectedFilePath
        {
            get
            {
                return m_selectedFilePath;
            }
            set
            {
                m_selectedFilePath = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public AddTexturePopupWindowViewModel() : base()
        {
            m_selectedFileName = "None";
            m_selectedFilePath = string.Empty;
        }

        public override void Clear()
        {
            base.Clear();
            m_width = 0;
            m_height = 0;
            m_selectedFileName = string.Empty;
            m_selectedFilePath = string.Empty;
        }

        private int m_width;
        private int m_height;
        private string m_selectedFileName;
        private string m_selectedFilePath;
    }
}
