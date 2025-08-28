using SevenWonders.GameEngine;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SevenWonders.SceneEditor.ViewModels
{
    public class TextureContentsViewModel: INotifyPropertyChanged
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
                        foreach (TextureListViewModel textureListViewModel in m_selectedLayer.Textures.Select(texture => new TextureListViewModel(texture)))
                        {
                            TextureViews.Add(textureListViewModel);
                        }
                    }
                }
            }
        }

        public Texture? SelectedTexture
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
                OnPropertyChanged(nameof(SelectedTextureWidth));
                OnPropertyChanged(nameof(SelectedTextureHeight));
            }
        }

        public bool IsSelectedTextureAvailable => SelectedTexture is not null;

        public string SelectedTextureName
        {
            get
            {
                return SelectedTexture?.Name ?? string.Empty;
            }
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
            get
            {
                return SelectedTexture?.Visible ?? false;
            }
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
            get
            {
                return SelectedTexture?.Position.X ?? -1;
            }
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
            get
            {
                return SelectedTexture?.Position.Y ?? -1;
            }
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
            get
            {
                return SelectedTexture?.Rotation ?? -1;
            }
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
            get
            {
                return SelectedTexture?.Scale.X ?? -1;
            }
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
            get
            {
                return SelectedTexture?.Scale.Y ?? -1;
            }
            set
            {
                if (SelectedTexture is not null)
                {
                    SelectedTexture.Scale = new Vector2(SelectedTexture.Scale.X, value);
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextureWidth
        {
            get
            {
                return SelectedTexture?.Width ?? -1;
            }
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
            get
            {
                return SelectedTexture?.Height ?? -1;
            }
            set
            {
                if (SelectedTexture is not null)
                {
                    SelectedTexture.Height = value;
                    OnPropertyChanged();
                }
            }
        }

        public TextureContentsViewModel()
        {
            TextureViews = new ObservableCollection<TextureListViewModel>();
            SelectedLayer = null;
            SelectedTexture = null;
        }

        public void AddTextureToLayer(string name, int id, bool visible, int textureId, int width, int height, string fullPath)
        {
            if (SelectedLayer is null)
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

            SelectedLayer.Textures.Add(texture);
            TextureViews.Add(new TextureListViewModel(texture));
            SelectedTexture = texture;
        }

        public void SetSelectedTexture(TextureListViewModel? textureListViewModel)
        {
            if (m_selectedLayer is null || textureListViewModel is null)
            {
                return;
            }

            SelectedTexture = m_selectedLayer.Textures.FirstOrDefault(texture => texture.Id == textureListViewModel.Id);
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

            m_selectedLayer.Textures.Remove(SelectedTexture);
            SelectedTexture = null;
        }

        private Texture? m_selectedTexture;
        private GraphicsLayer? m_selectedLayer;
    }
}
