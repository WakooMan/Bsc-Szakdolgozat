using SevenWonders.Game.Scene.Editor.ViewModels;
using System.Collections.ObjectModel;

namespace SevenWonders.Game.Scene.Editor.ViewModels
{
    public class AddTextureObjectPopupWindowViewModel : AddPopupWindowViewModel
    {
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

        protected override bool CanExecuteAdd()
        {
            return base.CanExecuteAdd() && m_selectedSceneTextureView is not null && m_width > 0 && m_height > 0;
        }

        public AddTextureObjectPopupWindowViewModel(ObservableCollection<SceneTextureListViewModel> sceneTextureViews) : base()
        {
            SceneTextureViews = sceneTextureViews;
        }

        public override void Clear()
        {
            base.Clear();
            m_selectedSceneTextureView = null;
            m_width = 0;
            m_height = 0;
        }

        private SceneTextureListViewModel? m_selectedSceneTextureView;
        private int m_width;
        private int m_height;
    }
}
