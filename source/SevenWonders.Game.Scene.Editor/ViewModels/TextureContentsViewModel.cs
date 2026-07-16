using SevenWonders.Game.Engine;
using SevenWonders.Game.Engine.SceneHandling;
using SevenWonders.Game.Engine.SceneObjects;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SevenWonders.Game.Scene.Editor.ViewModels
{
    public class TextureContentsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<TextureListViewModel> TextureViews { get; set; }

        public GraphicsLayer? SelectedLayer
        {
            get
            {
                return m_selectedLayer;
            }
            set
            {
                if (m_selectedLayer != value)
                {
                    m_selectedLayer = value;
                    SelectedTexture = null;
                    if (m_selectedLayer is not null)
                    {
                        TextureViews.Clear();
                        foreach (TextureListViewModel textureListViewModel in m_selectedLayer.TextureObjects.Select(texture => new TextureListViewModel(texture)))
                        {
                            TextureViews.Add(textureListViewModel);
                        }
                    }
                }
            }
        }

        public TextureObject? SelectedTexture
        {
            get
            {
                return m_selectedTexture;
            }
            set
            {
                m_selectedTexture = value;
                OnPropertyChanged(nameof(IsSelectedTextureAvailable));
                OnPropertyChanged(nameof(SelectedTextureName));
                OnPropertyChanged(nameof(SelectedTexturePositionX));
                OnPropertyChanged(nameof(SelectedTexturePositionY));
                OnPropertyChanged(nameof(SelectedTextureRotation));
                OnPropertyChanged(nameof(SelectedTextureScaleX));
                OnPropertyChanged(nameof(SelectedTextureScaleY));
                OnPropertyChanged(nameof(SelectedTextureZIndex));
                OnPropertyChanged(nameof(SelectedTextureWidth));
                OnPropertyChanged(nameof(SelectedTextureHeight));
                OnPropertyChanged(nameof(SelectedTextureView));
            }
        }

        public TextureListViewModel? SelectedTextureView
        {
            get
            {
                if (m_selectedTexture is null) return null;
                return TextureViews.FirstOrDefault(t => t.Id == m_selectedTexture.Id);
            }
            set
            {
                SetSelectedTexture(value);
            }
        }

        public bool IsSelectedTextureAvailable => SelectedTexture is not null;

        public string SelectedTextureName
        {
            get => SelectedTexture?.Name ?? string.Empty;
            set
            {
                if (SelectedTexture is not null && !string.IsNullOrWhiteSpace(value))
                {
                    SelectedTexture.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool SelectedTextureVisible
        {
            get => SelectedTexture?.Visible ?? false;
            set
            {
                if (SelectedTexture is not null)
                {
                    SelectedTexture.Visible = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTexturePositionX
        {
            get => SelectedTexture?.Position.X ?? -1;
            set
            {
                if (SelectedTexture is not null)
                {
                    SelectedTexture.Position = new Vector2(value, SelectedTexture.Position.Y);
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTexturePositionY
        {
            get => SelectedTexture?.Position.Y ?? -1;
            set
            {
                if (SelectedTexture is not null)
                {
                    SelectedTexture.Position = new Vector2(SelectedTexture.Position.X, value);
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextureRotation
        {
            get => SelectedTexture?.Rotation ?? -1;
            set
            {
                if (SelectedTexture is not null)
                {
                    SelectedTexture.Rotation = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextureScaleX
        {
            get => SelectedTexture?.Scale.X ?? -1;
            set
            {
                if (SelectedTexture is not null)
                {
                    SelectedTexture.Scale = new Vector2(value, SelectedTexture.Scale.Y);
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextureScaleY
        {
            get => SelectedTexture?.Scale.Y ?? -1;
            set
            {
                if (SelectedTexture is not null)
                {
                    SelectedTexture.Scale = new Vector2(SelectedTexture.Scale.X, value);
                    OnPropertyChanged();
                }
            }
        }

        public int SelectedTextureZIndex
        {
            get => SelectedTexture?.ZIndex ?? -1;
            set
            {
                if (SelectedTexture is not null)
                {
                    SelectedTexture.ZIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextureWidth
        {
            get => SelectedTexture?.Width ?? -1;
            set
            {
                if (SelectedTexture is not null)
                {
                    SelectedTexture.Width = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextureHeight
        {
            get => SelectedTexture?.Height ?? -1;
            set
            {
                if (SelectedTexture is not null)
                {
                    SelectedTexture.Height = value;
                    OnPropertyChanged();
                }
            }
        }

        public TextureContentsViewModel(IEngine engine)
        {
            m_engine = engine;
            TextureViews = new ObservableCollection<TextureListViewModel>();
            SelectedLayer = null;
            SelectedTexture = null;
        }

        /// <summary>
        /// Adds a <see cref="TextureObject"/> to the selected layer, referencing a scene-level
        /// <see cref="Texture"/> by <paramref name="textureId"/>.
        /// </summary>
        public void AddTextureObjectToLayer(string name, bool visible, int width, int height, int textureId)
        {
            if (m_engine.SceneManager.CurrentScene is null || SelectedLayer is null)
            {
                return;
            }

            TextureObject textureObject = new TextureObject()
            {
                Name = name,
                Position = new Vector2(0, 0),
                TextureId = textureId,
                Visible = visible,
                Width = width,
                Height = height,
                Scale = new Vector2(1, 1)
            };

            m_engine.ObjectManager.AddSceneObject(m_engine.SceneManager.CurrentScene, SelectedLayer, textureObject);
            TextureViews.Add(new TextureListViewModel(textureObject));
            SelectedTexture = textureObject;
        }

        public void SetSelectedTexture(TextureListViewModel? textureListViewModel)
        {
            if (m_selectedLayer is null || textureListViewModel is null)
            {
                return;
            }

            SelectedTexture = m_selectedLayer.TextureObjects.FirstOrDefault(texture => texture.Id == textureListViewModel.Id);
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void DeleteSelectedTexture()
        {
            if (m_selectedLayer is null || SelectedTexture is null)
            {
                return;
            }
            TextureListViewModel? textureListViewModel = TextureViews.FirstOrDefault(texture => texture.Id == SelectedTexture.Id);
            if (textureListViewModel is not null)
            {
                TextureViews.Remove(textureListViewModel);
            }

            m_engine.ObjectManager.RemoveSceneObject(m_selectedLayer, SelectedTexture);
            SelectedTexture = null;
        }

        private TextureObject? m_selectedTexture;
        private GraphicsLayer? m_selectedLayer;
        private readonly IEngine m_engine;
    }
}
