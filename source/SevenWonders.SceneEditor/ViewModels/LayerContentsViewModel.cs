using SevenWonders.GameEngine;
using SkiaSharp.Views.Maui;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SevenWonders.SceneEditor.ViewModels
{
    public class LayerContentsViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<LayerListViewModel> LayerViews { get; set; }

        public Scene? CurrentScene
        {
            get
            {
                return m_currentScene;
            }
            set
            {
                if (m_currentScene != value)
                {
                    m_currentScene = value;
                    SelectedLayer = null;
                    if (m_currentScene is not null)
                    {
                        LayerViews.Clear();
                        foreach (LayerListViewModel layerListViewModel in m_currentScene.Layers.Select(graphicsLayer => new LayerListViewModel(graphicsLayer)))
                        {
                            LayerViews.Add(layerListViewModel);
                        }
                    }
                }
            }
        }

        public GraphicsLayer? SelectedLayer
        {
            get
            {
                return m_selectedLayer;
            }
            set
            {
                m_selectedLayer = value;
                OnPropertyChanged(nameof(IsSelectedLayerAvailable));
                OnPropertyChanged(nameof(SelectedLayerName));
                OnPropertyChanged(nameof(SelectedLayerVisible));
                OnPropertyChanged(nameof(SelectedLayerEnableCollision));
                m_textureContentsViewModel.SelectedLayer = m_selectedLayer;
                m_gameObjectContentsViewModel.SelectedLayer = m_selectedLayer;
            }
        }

        public bool IsSelectedLayerAvailable => SelectedLayer is not null;

        public string SelectedLayerName
        {
            get
            {
                return SelectedLayer?.Name ?? string.Empty;
            }
            set
            {
                if (SelectedLayer is not null && !string.IsNullOrWhiteSpace(value))
                {
                    SelectedLayer.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool SelectedLayerVisible
        {
            get
            {
                return SelectedLayer?.Visible ?? false;
            }
            set
            {
                if (SelectedLayer is not null)
                {
                    SelectedLayer.Visible = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool SelectedLayerEnableCollision
        {
            get
            {
                return SelectedLayer?.EnableCollision ?? false;
            }
            set
            {
                if (SelectedLayer is not null)
                {
                    SelectedLayer.EnableCollision = value;
                    OnPropertyChanged();
                }
            }
        }


        public LayerContentsViewModel(TextureContentsViewModel textureContentsViewModel, GameObjectContentsViewModel gameObjectContentsView)
        {
            m_textureContentsViewModel = textureContentsViewModel;
            m_gameObjectContentsViewModel = gameObjectContentsView;
            LayerViews = new ObservableCollection<LayerListViewModel>();
            CurrentScene = null;
            SelectedLayer = null;
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
            };
            CurrentScene.Layers.Add(graphicsLayer);
            LayerViews.Add(new LayerListViewModel(graphicsLayer));
            SelectedLayer = graphicsLayer;
        }

        public void SetSelectedLayer(LayerListViewModel? layerViewModel)
        {
            if (m_currentScene is null || layerViewModel is null)
            {
                return;
            }

            SelectedLayer = m_currentScene.Layers.FirstOrDefault(layer => layer.ID == layerViewModel.Id);
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void DrawSelectedLayer(SKPaintSurfaceEventArgs eventArgs)
        {
            if (SelectedLayer is null)
            {
                return;
            }

            SelectedLayer.Draw(eventArgs);
        }

        public void DeleteSelectedLayer()
        {
            if (m_currentScene is null || SelectedLayer is null)
            {
                return;
            }
            LayerListViewModel? layerListViewModel = LayerViews.FirstOrDefault(layer => layer.Id == SelectedLayer.ID);
            if (layerListViewModel is not null)
            {
                LayerViews.Remove(layerListViewModel);
            }

            m_currentScene.Layers.Remove(SelectedLayer);
            SelectedLayer = null;
        }

        private GraphicsLayer? m_selectedLayer;
        private Scene? m_currentScene;
        private readonly TextureContentsViewModel m_textureContentsViewModel;
        private readonly GameObjectContentsViewModel m_gameObjectContentsViewModel;
    }
}
