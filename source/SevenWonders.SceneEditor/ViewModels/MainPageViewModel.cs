using SevenWonders.GameEngine;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SevenWonders.SceneEditor.ViewModels
{
    public enum MainWindowState
    {
        ButtonsWindow,
        CanvasWindow,
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<GameObjectListViewModel> GameObjectViews { get; set; }

        public string Name
        {
            get
            {
                return m_currentScene?.Name ?? string.Empty;
            }
            set
            {
                if (m_currentScene is null)
                {
                    return;
                }

                m_currentScene.Name = value;
                OnPropertyChanged();
            }
        }
        public bool IsVisible
        {
            get
            {
                return m_currentScene?.Visible ?? false;
            }
            set
            {
                if (m_currentScene is null)
                {
                    return;
                }

                m_currentScene.Visible = value;
                OnPropertyChanged();
            }
        }
        public int Id
        {
            get
            {
                return m_currentScene?.Id ?? -1;
            }
            set
            {
                if (m_currentScene is null)
                {
                    return;
                }

                m_currentScene.Id = value;
                OnPropertyChanged();
            }
        }

        public Scene? CurrentScene
        {
            get
            {
                return m_currentScene;
            }
            private set
            {
                m_currentScene = value;
                OnPropertyChanged(nameof(Id));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(IsVisible));
                LayerContentsViewModel.CurrentScene = m_currentScene;
            }
        }

        public LayerContentsViewModel LayerContentsViewModel
        {
            get
            {
                return m_layerContentsViewModel;
            }
            private set
            {
                m_layerContentsViewModel = value;
                OnPropertyChanged();
            }
        }

        public TextureContentsViewModel TextureContentsViewModel
        {
            get
            {
                return m_textureContentsViewModel;
            }
            private set
            {
                m_textureContentsViewModel = value;
                OnPropertyChanged();
            }
        }

        public bool CanvasIsVisible
        {
            get
            {
                return m_canvasIsVisible;
            }
            set
            {
                m_canvasIsVisible = value;
                OnPropertyChanged();
            }
        }

        public bool ButtonsAreVisible
        {
            get
            {
                return m_buttonsAreVisible;
            }
            set
            {
                m_buttonsAreVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsLeftPanelVisible
        {
            get
            {
                return m_isLeftPanelVisible;
            }
            set
            {
                m_isLeftPanelVisible = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel()
        {
            m_textureContentsViewModel = new TextureContentsViewModel();
            m_layerContentsViewModel = new LayerContentsViewModel(m_textureContentsViewModel);
            CurrentScene = null;
            SetState(MainWindowState.ButtonsWindow);
            GameObjectViews = new ObservableCollection<GameObjectListViewModel>();
        }

        public void SetCurrentScene(string name, int id, bool visible)
        {
            if (CurrentScene is not null)
            {
                return;
            }

            Scene scene = new Scene()
            {
                Name = name,
                Id = id,
                Visible = visible
            };
            CurrentScene = scene;
            SetState(MainWindowState.CanvasWindow);
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void SetState(MainWindowState mainWindowState)
        {
            m_state = mainWindowState;
            CanvasIsVisible = m_state == MainWindowState.CanvasWindow ? true : false;
            IsLeftPanelVisible = CanvasIsVisible;
            ButtonsAreVisible = m_state == MainWindowState.ButtonsWindow ? true : false;
        }

        private LayerContentsViewModel m_layerContentsViewModel;
        private TextureContentsViewModel m_textureContentsViewModel;
        private Scene? m_currentScene;
        private bool m_canvasIsVisible;
        private bool m_isLeftPanelVisible;
        private bool m_buttonsAreVisible;
        private MainWindowState m_state;
    }
}
