using System.Collections.ObjectModel;

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

        public AddSpritePopupWindowViewModel(ObservableCollection<SceneTextureListViewModel> sceneTextureViews) : base()
        {
            SceneTextureViews = sceneTextureViews;
            m_frameHeight = 0;
            m_frameWidth = 0;
            m_rows = 0;
            m_columns = 0;
            m_selectedSceneTextureView = null;
        }

        protected override bool CanExecuteAdd()
        {
            return base.CanExecuteAdd() && m_selectedSceneTextureView is not null && m_frameHeight > 0 && m_frameWidth > 0 && m_rows > 0 && m_columns > 0; // TODO: Check if it is a valid texture ID
        }

        public override void Clear()
        {
            base.Clear();
            m_frameHeight = 0;
            m_frameWidth = 0;
            m_rows = 0;
            m_columns = 0;
            m_selectedSceneTextureView = null;
        }

        private SceneTextureListViewModel? m_selectedSceneTextureView;
        private int m_frameWidth;
        private int m_frameHeight;
        private int m_rows;
        private int m_columns;
        
    }
}
