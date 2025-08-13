using SevenWonders.GameEngine;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SevenWonders.SceneEditor
{
    public enum MainWindowState
    {
        ButtonsWindow,
        AddSceneWindow,
        CanvasWindow
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public MainPageViewModel()
        {
            AddSceneViewModel = new AddSceneViewModel();
            CurrentScene = null;
            SetState(MainWindowState.ButtonsWindow);
            OnAddSceneCommand = new Command(OnAddSceneCommandExecute);
            OnAddCommand = new Command(OnAddCommandExecute);
            OnBackCommand = new Command(OnBackCommandExecute);
        }

        public AddSceneViewModel AddSceneViewModel { get; set; }
        public ICommand OnAddSceneCommand { get; set; }
        public ICommand OnAddCommand { get; set; }
        public ICommand OnBackCommand { get; set; }
        public ICommand OnAddLayer { get; set; }
        public ICommand OnAddGameObject { get; set; }
        public ICommand OnAddTexture { get; set; }

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
            set
            {
                m_currentScene = value;
                OnPropertyChanged(nameof(Id));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(IsVisible));
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

        public bool AddSceneVisible
        {
            get
            {
                return m_addSceneVisible;
            }
            set
            {
                m_addSceneVisible = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void SetState(MainWindowState mainWindowState)
        {
            m_state = mainWindowState;
            CanvasIsVisible = m_state == MainWindowState.CanvasWindow ? true : false;
            ButtonsAreVisible = m_state == MainWindowState.ButtonsWindow ? true : false;
            AddSceneVisible = m_state == MainWindowState.AddSceneWindow ? true : false;
        }

        private void OnAddSceneCommandExecute()
        {
            SetState(MainWindowState.AddSceneWindow);
        }

        private void OnBackCommandExecute()
        {
            AddSceneViewModel.SceneName = string.Empty;
            AddSceneViewModel.SceneId = 0;
            SetState(MainWindowState.ButtonsWindow);
        }

        private void OnAddCommandExecute()
        {
            CurrentScene = new Scene()
            {
                Name = AddSceneViewModel.SceneName,
                Id = AddSceneViewModel.SceneId,
                Visible = true
            };
            AddSceneViewModel.SceneName = string.Empty;
            AddSceneViewModel.SceneId = 0;
            SetState(MainWindowState.CanvasWindow);
        }

        private void OnAddLayerExecute()
        {
        }

        private Scene? m_currentScene;
        private bool m_canvasIsVisible;
        private bool m_buttonsAreVisible;
        private bool m_addSceneVisible;
        private MainWindowState m_state;
    }
}
