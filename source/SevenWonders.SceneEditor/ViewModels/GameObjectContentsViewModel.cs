using SevenWonders.GameEngine;
using SevenWonders.SceneEditor.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.SceneEditor.ViewModels
{
    public class GameObjectContentsViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<GameObjectListViewModel> GameObjectViews { get; set; }
        public ObservableCollection<SpriteListViewModel> SpriteViews { get; set; }

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
                    SelectedGameObject = null;
                    if (m_selectedLayer is not null)
                    {
                        GameObjectViews.Clear();
                        foreach (GameObjectListViewModel gameObjectListViewModel in m_selectedLayer.ObjectList.Select(gameObject => new GameObjectListViewModel(gameObject)))
                        {
                            GameObjectViews.Add(gameObjectListViewModel);
                        }
                    }
                }
            }
        }

        public GameObject? SelectedGameObject
        {
            get
            {
                return m_selectedGameObject;
            }
            set
            {
                m_selectedGameObject = value;
                SpriteViews.Clear();
                OnPropertyChanged(nameof(IsSelectedGameObjectAvailable));
                OnPropertyChanged(nameof(SelectedGameObjectName));
                OnPropertyChanged(nameof(SelectedGameObjectPositionX));
                OnPropertyChanged(nameof(SelectedGameObjectPositionY));
                OnPropertyChanged(nameof(SelectedGameObjectRotation));
                OnPropertyChanged(nameof(SelectedGameObjectScaleX));
                OnPropertyChanged(nameof(SelectedGameObjectScaleY));
                if (m_selectedGameObject is not null)
                {
                    m_selectedGameObject.Animations.ForEach(animation => SpriteViews.Add(new SpriteListViewModel(animation)));
                }
            }
        }

        public bool IsSelectedGameObjectAvailable => SelectedGameObject is not null;

        public string SelectedGameObjectName
        {
            get
            {
                return SelectedGameObject?.Name ?? string.Empty;
            }
            set
            {
                if (SelectedGameObject is not null && !string.IsNullOrWhiteSpace(value))
                {
                    SelectedGameObject.Name = value;
                    OnPropertyChanged();
                }
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
                IsCopyEnabled = !string.IsNullOrEmpty(m_copyName) && SelectedGameObjectName != m_copyName;
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

        public bool SelectedGameObjectVisible
        {
            get
            {
                return SelectedGameObject?.Visible ?? false;
            }
            set
            {
                if (SelectedGameObject is not null)
                {
                    SelectedGameObject.Visible = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedGameObjectPositionX
        {
            get
            {
                return SelectedGameObject?.Position.X ?? -1;
            }
            set
            {
                if (SelectedGameObject is not null)
                {
                    SelectedGameObject.Position = new Vector2(value, SelectedGameObject.Position.Y);
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedGameObjectPositionY
        {
            get
            {
                return SelectedGameObject?.Position.Y ?? -1;
            }
            set
            {
                if (SelectedGameObject is not null)
                {
                    SelectedGameObject.Position = new Vector2(SelectedGameObject.Position.X, value);
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedGameObjectRotation
        {
            get
            {
                return SelectedGameObject?.Rotation ?? -1;
            }
            set
            {
                if (SelectedGameObject is not null)
                {
                    SelectedGameObject.Rotation = value;
                    OnPropertyChanged();
                }
            }
        }


        public float SelectedGameObjectScaleX
        {
            get
            {
                return SelectedGameObject?.Scale.X ?? -1;
            }
            set
            {
                if (SelectedGameObject is not null)
                {
                    SelectedGameObject.Scale = new Vector2(value, SelectedGameObject.Scale.Y);
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedGameObjectScaleY
        {
            get
            {
                return SelectedGameObject?.Scale.Y ?? -1;
            }
            set
            {
                if (SelectedGameObject is not null)
                {
                    SelectedGameObject.Scale = new Vector2(SelectedGameObject.Scale.X, value);
                    OnPropertyChanged();
                }
            }
        }

        public GameObjectContentsViewModel()
        {
            GameObjectViews = new ObservableCollection<GameObjectListViewModel>();
            SpriteViews = new ObservableCollection<SpriteListViewModel>();
            SelectedLayer = null;
            SelectedGameObject = null;
        }

        public void AddGameObjectToLayer(string name, int id, bool visible)
        {
            if (SelectedLayer is null)
            {
                return;
            }

            GameObject gameObject = new GameObject()
            {
                Name = name,
                Id = id,
                Position = new Vector2(0, 0),
                Visible = visible,
                Scale = new Vector2(1, 1)
            };
            gameObject.LoadTextures(FileHelper.TempPath);

            SelectedLayer.ObjectList.Add(gameObject);
            GameObjectViews.Add(new GameObjectListViewModel(gameObject));
            SelectedGameObject = gameObject;
        }

        public void SetSelectedGameObject(GameObjectListViewModel? gameObjectListViewModel)
        {
            if (m_selectedLayer is null || gameObjectListViewModel is null)
            {
                return;
            }

            SelectedGameObject = m_selectedLayer.ObjectList.FirstOrDefault(gameObject => gameObject.Id == gameObjectListViewModel.Id);
        }

        public void SetSelectedSprite(SpriteListViewModel? spriteListViewModel)
        {
            if (m_selectedLayer is null || spriteListViewModel is null || m_selectedGameObject is null)
            {
                return;
            }

            m_selectedGameObject.CurrentAnim = m_selectedGameObject.Animations.FindIndex(sprite => sprite == spriteListViewModel.Sprite);
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void DeleteSelectedGameObject()
        {
            if (m_selectedLayer is null || SelectedGameObject is null)
            {
                return;
            }
            GameObjectListViewModel? gameObjectListViewModel = GameObjectViews.FirstOrDefault(gameObject => gameObject.Id == SelectedGameObject.Id);
            if (gameObjectListViewModel is not null)
            {
                GameObjectViews.Remove(gameObjectListViewModel);
            }

            m_selectedLayer.ObjectList.Remove(SelectedGameObject);
            SelectedGameObject = null;
        }

        public void CopySelectedGameObject()
        {
            if (SelectedLayer is null || m_selectedGameObject is null)
            {
                return;
            }

            GameObject gameObject = new GameObject(m_selectedGameObject);
            gameObject.Name = new string(CopyName);
            gameObject.Id = m_selectedGameObject.Id + 1;
            gameObject.LoadTextures(FileHelper.TempPath);

            SelectedLayer.ObjectList.Add(gameObject);
            GameObjectViews.Add(new GameObjectListViewModel(gameObject));
            SelectedGameObject = gameObject;
        }

        public void DeleteSelectedSprite()
        {
            if (m_selectedGameObject is null)
                return;

            Sprite sprite = m_selectedGameObject.Animations[m_selectedGameObject.CurrentAnim];
            m_selectedGameObject.Animations.Remove(sprite);
            SpriteListViewModel? spriteListViewModel = SpriteViews.FirstOrDefault(sprite => sprite.Name == sprite.Name);
            if (spriteListViewModel is not null)
            {
                SpriteViews.Remove(spriteListViewModel);
            }

            m_selectedGameObject.CurrentAnim = 0;
        }

        public void AddSpriteToGameObject(string name, string textureName, int idOfTexture, bool visible, int textureId, int width, int height, string fullPath, int frameHeight, int frameWidth, int rows, int columns)
        {
            if (m_selectedGameObject is null)
            {
                return;
            }

            string fileName = Path.GetFileName(fullPath);
            string destinationFileName = Path.Combine(FileHelper.TempPath, fileName);
            if (!File.Exists(destinationFileName))
            {
                File.Copy(fullPath, destinationFileName);
            }

            Texture texture = new Texture()
            {
                Name = textureName,
                Id = idOfTexture,
                Position = new Vector2(0, 0),
                Color = SKColor.Empty,
                TextureId = textureId,
                FileName = fileName,
                Visible = visible,
                Width = width,
                Height = height,
                Scale = new Vector2(1, 1)
            };
            texture.LoadTexture(FileHelper.TempPath);

            List<SpriteFrame> frames = new List<SpriteFrame>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    frames.Add(new SpriteFrame()
                    {
                        Frame = texture,
                        Name = name,
                        Left = j * frameWidth,
                        Right = (j + 1) * frameWidth,
                        Top = i * frameHeight,
                        Bottom = (i + 1) * frameHeight,
                    });
                }
            }

            Sprite sprite = new Sprite()
            {
                ActualFrame = 0,
                Name = name,
                Fps = 60,
                Frames = frames,
                NumFrames = frames.Count,
                LoopAnimation = true,
                RotationZ = 0,
                LastUpdate = 0,
            };

            m_selectedGameObject.Animations.Add(sprite);
        }

        private GameObject? m_selectedGameObject;
        private GraphicsLayer? m_selectedLayer;
        private string m_copyName;
        private bool m_isCopyEnabled;
    }
}
