using SevenWonders.GameEngine;
using SkiaSharp.Views.Maui;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SevenWonders.SceneEditor.ViewModels
{
    public class LayerContentsViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<LayerListViewModel> LayerViews { get; set; }
        public ICommand OnUnselectLayerCommand => m_onUnselectLayerCommand;

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
                    m_onUnselectLayerCommand.ChangeCanExecute();
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
                OnPropertyChanged(nameof(SelectedLayerZIndex));
                OnPropertyChanged(nameof(SelectedLayerView));
                m_textureContentsViewModel.SelectedLayer = m_selectedLayer;
                m_gameObjectContentsViewModel.SelectedLayer = m_selectedLayer;
                m_buttonContentsViewModel.SelectedLayer = m_selectedLayer;
                m_textLabelContentsViewModel.SelectedLayer = m_selectedLayer;
                m_onUnselectLayerCommand.ChangeCanExecute();
            }
        }

        public LayerListViewModel? SelectedLayerView
        {
            get
            {
                if (m_selectedLayer is null) return null;
                return LayerViews.FirstOrDefault(l => l.Id == m_selectedLayer.Id);
            }
            set
            {
                SetSelectedLayer(value);
            }
        }

        public string CopyName
        {
            get
            {
                return m_copyName;
            }
            set
            {
                m_copyName = value;
                OnPropertyChanged();
                IsCopyEnabled = !string.IsNullOrEmpty(m_copyName) && SelectedLayerName != m_copyName;
            }
        }

        public bool IsCopyEnabled
        {
            get
            {
                return m_isCopyEnabled;
            }
            set
            {
                m_isCopyEnabled = value;
                OnPropertyChanged();
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

        public int SelectedLayerZIndex
        {
            get
            {
                return SelectedLayer?.ZIndex ?? 0;
            }
            set
            {
                if (SelectedLayer is not null)
                {
                    SelectedLayer.ZIndex = value;
                    OnPropertyChanged();
                }
            }
        }


        public LayerContentsViewModel(IEngine engine, TextureContentsViewModel textureContentsViewModel, GameObjectContentsViewModel gameObjectContentsViewModel, ButtonContentsViewModel buttonContentsViewModel, TextLabelContentsViewModel textLabelContentsViewModel)
        {
            m_engine = engine;
            m_textureContentsViewModel = textureContentsViewModel;
            m_gameObjectContentsViewModel = gameObjectContentsViewModel;
            m_buttonContentsViewModel = buttonContentsViewModel;
            m_textLabelContentsViewModel = textLabelContentsViewModel;
            LayerViews = new ObservableCollection<LayerListViewModel>();
            m_onUnselectLayerCommand = new Command(OnUnselectLayer, () => CurrentScene is not null && SelectedLayer is not null);
            CurrentScene = null;
            SelectedLayer = null;
            m_copyName = string.Empty;
        }

        public void AddLayer(string name, bool visible)
        {
            if (CurrentScene is null)
            {
                return;
            }

            GraphicsLayer graphicsLayer = new GraphicsLayer()
            {
                Name = name,
                Visible = visible,
                EnableCollision = true,
            };

            m_engine.ObjectManager.AddGraphicsLayer(CurrentScene, graphicsLayer);
            LayerViews.Add(new LayerListViewModel(graphicsLayer));
            SelectedLayer = graphicsLayer;
        }

        public void SetSelectedLayer(LayerListViewModel? layerViewModel)
        {
            if (m_currentScene is null || layerViewModel is null)
            {
                return;
            }

            SelectedLayer = m_currentScene.Layers.FirstOrDefault(layer => layer.Id == layerViewModel.Id);
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void DrawSelectedLayer(SKPaintSurfaceEventArgs eventArgs, TextureRegistry textureRegistry)
        {
            if (SelectedLayer is null)
            {
                return;
            }

            SelectedLayer.Draw(eventArgs.Surface.Canvas, textureRegistry);
        }

        public void DeleteSelectedLayer()
        {
            if (m_currentScene is null || SelectedLayer is null)
            {
                return;
            }
            LayerListViewModel? layerListViewModel = LayerViews.FirstOrDefault(layer => layer.Id == SelectedLayer.Id);
            if (layerListViewModel is not null)
            {
                LayerViews.Remove(layerListViewModel);
            }

            m_currentScene.Layers.Remove(SelectedLayer);
            SelectedLayer = null;
        }

        public void CopySelectedLayer()
        {
            if (m_selectedLayer is null || m_currentScene is null)
            {
                return;
            }

            GraphicsLayer layer = m_engine.ObjectManager.CopyGraphicsLayer(m_currentScene, m_selectedLayer, CopyName);
            LayerViews.Add(new LayerListViewModel(layer));
            SelectedLayer = layer;
        }

        private void OnUnselectLayer()
        {
            SelectedLayer = null;
        }

        private Command m_onUnselectLayerCommand;
        private GraphicsLayer? m_selectedLayer;
        private Scene? m_currentScene;
        private readonly TextureContentsViewModel m_textureContentsViewModel;
        private readonly GameObjectContentsViewModel m_gameObjectContentsViewModel;
        private readonly ButtonContentsViewModel m_buttonContentsViewModel;
        private readonly TextLabelContentsViewModel m_textLabelContentsViewModel;
        private readonly IEngine m_engine;
        private string m_copyName;
        private bool m_isCopyEnabled;
    }
}
