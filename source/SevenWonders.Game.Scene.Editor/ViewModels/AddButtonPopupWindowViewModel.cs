using System.Collections.ObjectModel;

namespace SevenWonders.Game.Scene.Editor.ViewModels
{
    public class AddButtonPopupWindowViewModel : AddPopupWindowViewModel
    {
        protected override bool CanExecuteAdd()
        {
            return base.CanExecuteAdd() && m_selectedSceneTextureView is not null && m_width > 0 && m_height > 0;
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

        public ObservableCollection<SceneTextureListViewModel> SceneTextureViews { get; }

        public SceneTextureListViewModel? SelectedSceneTextureView
        {
            get => m_selectedSceneTextureView;
            set
            {
                m_selectedSceneTextureView = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public int SelectedTextureId => m_selectedSceneTextureView?.Id ?? -1;

        public AddButtonPopupWindowViewModel(ObservableCollection<SceneTextureListViewModel> sceneTextureViews) : base()
        {
            SceneTextureViews = sceneTextureViews;
            m_buttonText = string.Empty;
            m_fontSize = 24f;
        }

        public override void Clear()
        {
            base.Clear();
            m_buttonText = string.Empty;
            m_fontSize = 24f;
            m_width = 0;
            m_height = 0;
            m_selectedSceneTextureView = null;
        }

        private SceneTextureListViewModel? m_selectedSceneTextureView;
        private string m_buttonText;
        private float m_fontSize;
        private int m_width;
        private int m_height;
    }
}
