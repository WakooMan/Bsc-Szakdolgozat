using SevenWonders.GameEngine;
using SevenWonders.SceneEditor.Helpers;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Serialization;

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
        public ICommand OnSceneSaveCommand => m_onSceneSaveCommand;

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
                string tempPath = Path.Combine(Directory.GetCurrentDirectory(), "temp");
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
                Directory.CreateDirectory(tempPath);
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

        public GameObjectContentsViewModel GameObjectContentsViewModel
        {
            get
            {
                return m_gameObjectContentsViewModel;
            }
            private set
            {
                m_gameObjectContentsViewModel = value;
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
            m_gameObjectContentsViewModel = new GameObjectContentsViewModel();
            m_layerContentsViewModel = new LayerContentsViewModel(m_textureContentsViewModel, m_gameObjectContentsViewModel);
            CurrentScene = null;
            SetState(MainWindowState.ButtonsWindow);
            GameObjectViews = new ObservableCollection<GameObjectListViewModel>();
            m_onSceneSaveCommand = new Command(OnSceneSaveCommandExecute, () => CurrentScene is not null);
        }

        public void SetCurrentScene(Scene scene)
        {
            if (CurrentScene is not null)
            {
                return;
            }

            CurrentScene = scene;
            m_onSceneSaveCommand.ChangeCanExecute();
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

        private void OnSceneSaveCommandExecute()
        {
            if(m_currentScene is null)
                return;

            string scenesPath = FileHelper.ScenesPath;
            if (!Directory.Exists(scenesPath))
            {
                Directory.CreateDirectory(scenesPath);
            }

            string scenePath = Path.Combine(FileHelper.TempPath, "scene.xml");
            FileHelper.Serialize(m_currentScene, scenePath);

            string zipPath = Path.Combine(scenesPath, $"{m_currentScene.Name}.zip");

            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            ZipFile.CreateFromDirectory(
               FileHelper.TempPath,
               zipPath,
               CompressionLevel.Optimal,
               includeBaseDirectory: false);

            CurrentScene = null;
        }

        private Command m_onSceneSaveCommand;
        private LayerContentsViewModel m_layerContentsViewModel;
        private TextureContentsViewModel m_textureContentsViewModel;
        private GameObjectContentsViewModel m_gameObjectContentsViewModel;
        private Scene? m_currentScene;
        private bool m_canvasIsVisible;
        private bool m_isLeftPanelVisible;
        private bool m_buttonsAreVisible;
        private MainWindowState m_state;
    }
}
