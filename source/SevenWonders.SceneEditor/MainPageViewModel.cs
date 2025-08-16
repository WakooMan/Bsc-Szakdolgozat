using SevenWonders.GameEngine;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SevenWonders.SceneEditor
{
    public enum MainWindowState
    {
        ButtonsWindow,
        AddSceneWindow,
        AddLayerWindow,
        CanvasWindow,
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public MainPageViewModel()
        {
            AddSceneViewModel = new AddSceneViewModel();
            AddLayerViewModel = new AddLayerViewModel();
            CurrentScene = null;
            SetState(MainWindowState.ButtonsWindow);
            OnAddSceneCommand = new Command(OnAddSceneCommandExecute);
            OnAddCommand = new Command(OnAddCommandExecute);
            OnBackCommand = new Command(OnBackCommandExecute);
            OnAddLayer = new Command(OnAddLayerExecute);
            OnLayerAddCommand = new Command(OnLayerAddCommandExecute);
            OnLayerBackCommand = new Command(OnLayerBackCommandExecute);
            LayerViews = new ObservableCollection<LayerViewModel>();
        }

        public AddSceneViewModel AddSceneViewModel { get; set; }
        public AddLayerViewModel AddLayerViewModel { get; set; }
        public ICommand OnAddSceneCommand { get; set; }
        public ICommand OnAddCommand { get; set; }
        public ICommand OnBackCommand { get; set; }
        public ICommand OnAddLayer { get; set; }
        public ICommand OnAddGameObject { get; set; }
        public ICommand OnAddTexture { get; set; }
        public ICommand OnLayerAddCommand { get; set; }
        public ICommand OnLayerBackCommand { get; set; }

        public ObservableCollection<LayerViewModel> LayerViews { get; set; }

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

        public bool AddLayerVisible
        {
            get
            {
                return m_addLayerVisible;
            }
            set
            {
                m_addLayerVisible = value;
                OnPropertyChanged();
            }
        }

        public void SetSelectedLayer(LayerViewModel? layerViewModel)
        {
            if (m_currentScene is null || layerViewModel is null)
            {
                return;
            }

            m_selectedLayer = m_currentScene.Layers.FirstOrDefault(layer => layer.ID == layerViewModel.Id);
        }

        public void AddTextureToLayer(string fullPath, string fileName)
        {
            if (m_selectedLayer is null)
            {
                return;
            }
            Texture texture = new Texture()
            {
                Name = fileName,
                Id = 0,
                Position = new Vector2(0, 0),
                Color = SKColor.Empty,
                TextureId = 0,
                FileName = fullPath,
                Visible = true,
                Width = 1600,
                Height = 900,
                Scale = new Vector2(1, 1)
            };
            texture.LoadTexture();

            m_selectedLayer.Textures.Add(texture);
        }

        public void DrawSelectedLayer(SKPaintSurfaceEventArgs eventArgs)
        {
            if (m_selectedLayer is null)
            {
                return;
            }

            m_selectedLayer.Draw(eventArgs);
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void SetState(MainWindowState mainWindowState)
        {
            m_state = mainWindowState;
            CanvasIsVisible = m_state == MainWindowState.CanvasWindow ? true : false;
            ButtonsAreVisible = m_state == MainWindowState.ButtonsWindow ? true : false;
            AddSceneVisible = m_state == MainWindowState.AddSceneWindow ? true : false;
            AddLayerVisible = m_state == MainWindowState.AddLayerWindow ? true : false;
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
            if (CurrentScene is null)
            {
                return;
            }

            SetState(MainWindowState.AddLayerWindow);
        }

        private void OnLayerAddCommandExecute()
        {
            if (CurrentScene is null)
            {
                return;
            }

            GraphicsLayer graphicsLayer = new GraphicsLayer()
            {
                Name = AddLayerViewModel.LayerName,
                ID = AddLayerViewModel.LayerId,
                Visible = true,
                EnableCollision = true,
                ParentScene = CurrentScene,
            };
            CurrentScene.Layers.Add(graphicsLayer);
            LayerViews.Add(new LayerViewModel(graphicsLayer));
            m_selectedLayer = graphicsLayer;
            SetState(MainWindowState.CanvasWindow);
        }

        private void OnLayerBackCommandExecute()
        {
            AddLayerViewModel.LayerName = string.Empty;
            AddLayerViewModel.LayerId = 0;
            SetState(MainWindowState.CanvasWindow);
        }

        private GraphicsLayer? m_selectedLayer;
        private Scene? m_currentScene;
        private bool m_canvasIsVisible;
        private bool m_buttonsAreVisible;
        private bool m_addSceneVisible;
        private bool m_addLayerVisible;
        private MainWindowState m_state;
    }
}
