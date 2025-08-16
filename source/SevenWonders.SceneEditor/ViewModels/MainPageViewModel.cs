using SevenWonders.GameEngine;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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

        public MainPageViewModel()
        {
            CurrentScene = null;
            SetState(MainWindowState.ButtonsWindow);
            LayerViews = new ObservableCollection<LayerViewModel>();
        }
        public ICommand OnAddGameObject { get; set; }
        public ICommand OnAddTexture { get; set; }

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
            private set
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

        public void AddLayer(string name, int id, bool visible)
        {
            if (CurrentScene is null)
            {
                return;
            }

            GraphicsLayer graphicsLayer = new GraphicsLayer()
            {
                Name = name,
                ID = id,
                Visible = visible,
                EnableCollision = true,
                ParentScene = CurrentScene,
            };
            CurrentScene.Layers.Add(graphicsLayer);
            LayerViews.Add(new LayerViewModel(graphicsLayer));
            m_selectedLayer = graphicsLayer;
        }


        public void SetSelectedLayer(LayerViewModel? layerViewModel)
        {
            if (m_currentScene is null || layerViewModel is null)
            {
                return;
            }

            m_selectedLayer = m_currentScene.Layers.FirstOrDefault(layer => layer.ID == layerViewModel.Id);
        }

        public void AddTextureToLayer(string name, int id, bool visible, int textureId, int width, int height, string fullPath)
        {
            if (m_selectedLayer is null)
            {
                return;
            }
            Texture texture = new Texture()
            {
                Name = name,
                Id = id,
                Position = new Vector2(0, 0),
                Color = SKColor.Empty,
                TextureId = textureId,
                FilePath = fullPath,
                Visible = visible,
                Width = width,
                Height = height,
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
