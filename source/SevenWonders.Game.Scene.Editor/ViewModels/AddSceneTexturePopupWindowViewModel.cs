namespace SevenWonders.Game.Scene.Editor.ViewModels
{
    public class AddSceneTexturePopupWindowViewModel : AddPopupWindowViewModel
    {
        protected override bool CanExecuteAdd()
        {
            return !string.IsNullOrEmpty(m_selectedFilePath);
        }

        public string SelectedFileName
        {
            get => m_selectedFileName;
            set
            {
                m_selectedFileName = value;
                OnPropertyChanged();
            }
        }

        public string SelectedFilePath
        {
            get => m_selectedFilePath;
            set
            {
                m_selectedFilePath = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public AddSceneTexturePopupWindowViewModel() : base()
        {
            m_selectedFileName = "None";
            m_selectedFilePath = string.Empty;
        }

        public override void Clear()
        {
            base.Clear();
            m_selectedFileName = string.Empty;
            m_selectedFilePath = string.Empty;
        }

        private string m_selectedFileName;
        private string m_selectedFilePath;
    }
}
