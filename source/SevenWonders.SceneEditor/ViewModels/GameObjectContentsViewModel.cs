using SevenWonders.GameEngine;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

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
                OnPropertyChanged(nameof(SelectedGameObjectZIndex));
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

        public float SelectedGameObjectWidth
        {
            get
            {
                return SelectedGameObject?.Width ?? 0;
            }
            set
            {
                if (SelectedGameObject is not null)
                {
                    SelectedGameObject.Width = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedGameObjectHeight
        {
            get
            {
                return SelectedGameObject?.Height ?? 0;
            }
            set
            {
                if (SelectedGameObject is not null)
                {
                    SelectedGameObject.Height = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SelectedGameObjectZIndex
        {
            get
            {
                return SelectedGameObject?.ZIndex ?? 0;
            }
            set
            {
                if (SelectedGameObject is not null)
                {
                    SelectedGameObject.ZIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        public GameObjectContentsViewModel(IEngine engine)
        {
            m_engine = engine;
            GameObjectViews = new ObservableCollection<GameObjectListViewModel>();
            SpriteViews = new ObservableCollection<SpriteListViewModel>();
            m_copyName = string.Empty;
            SelectedLayer = null;
            SelectedGameObject = null;
        }

        public void AddGameObjectToLayer(string name, bool visible)
        {
            if (SelectedLayer is null || m_engine.SceneManager.CurrentScene is null)
            {
                return;
            }

            GameObject gameObject = new GameObject()
            {
                Name = name,
                Position = new Vector2(0, 0),
                Visible = visible,
                Scale = new Vector2(1, 1),
                ZIndex = 0
            };
            m_engine.ObjectManager.AddGameObject(m_engine.SceneManager.CurrentScene, SelectedLayer, gameObject);
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
            if (m_engine.SceneManager.CurrentScene is null || SelectedLayer is null || m_selectedGameObject is null)
            {
                return;
            }

            GameObject gameObject = m_engine.ObjectManager.CopyGameObject(m_engine.SceneManager.CurrentScene, SelectedLayer, m_selectedGameObject, CopyName);
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

        public void AddSpriteToGameObject(string name, string textureName, bool visible, int width, int height, string fullPath, int frameHeight, int frameWidth, int rows, int columns)
        {
            if (m_engine.SceneManager.CurrentScene is null || m_selectedGameObject is null)
            {
                return;
            }

            string fileName = Path.GetFileName(fullPath);
            string sceneFolderPath = m_engine.SceneFileHandler.ReceiveSceneFolder(m_engine.SceneManager.CurrentScene);
            string destinationFileName = Path.Combine(sceneFolderPath, fileName);
            if (!File.Exists(destinationFileName))
            {
                File.Copy(fullPath, destinationFileName);
            }

            Texture texture = new Texture()
            {
                Color = SKColor.Empty,
                FileName = fileName,
            };

            texture.LoadTexture(sceneFolderPath);

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

        private readonly IEngine m_engine;
        private GameObject? m_selectedGameObject;
        private GraphicsLayer? m_selectedLayer;
        private string m_copyName;
        private bool m_isCopyEnabled;
    }
}
