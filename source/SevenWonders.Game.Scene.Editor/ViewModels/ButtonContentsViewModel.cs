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
    public class ButtonContentsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<ButtonListViewModel> ButtonViews { get; set; }

        public GraphicsLayer? SelectedLayer
        {
            get => m_selectedLayer;
            set
            {
                if (m_selectedLayer != value)
                {
                    m_selectedLayer = value;
                    SelectedButton = null;
                    if (m_selectedLayer is not null)
                    {
                        ButtonViews.Clear();
                        foreach (ButtonListViewModel buttonListViewModel in m_selectedLayer.ButtonObjects.Select(button => new ButtonListViewModel(button)))
                        {
                            ButtonViews.Add(buttonListViewModel);
                        }
                    }
                }
            }
        }

        public ButtonObject? SelectedButton
        {
            get => m_selectedButton;
            set
            {
                m_selectedButton = value;
                OnPropertyChanged(nameof(IsSelectedButtonAvailable));
                OnPropertyChanged(nameof(SelectedButtonName));
                OnPropertyChanged(nameof(SelectedButtonVisible));
                OnPropertyChanged(nameof(SelectedButtonPositionX));
                OnPropertyChanged(nameof(SelectedButtonPositionY));
                OnPropertyChanged(nameof(SelectedButtonRotation));
                OnPropertyChanged(nameof(SelectedButtonScaleX));
                OnPropertyChanged(nameof(SelectedButtonScaleY));
                OnPropertyChanged(nameof(SelectedButtonZIndex));
                OnPropertyChanged(nameof(SelectedButtonWidth));
                OnPropertyChanged(nameof(SelectedButtonHeight));
                OnPropertyChanged(nameof(SelectedButtonText));
                OnPropertyChanged(nameof(SelectedButtonFontSize));
                OnPropertyChanged(nameof(SelectedButtonTextColorHex));
                OnPropertyChanged(nameof(SelectedButtonView));
            }
        }

        public ButtonListViewModel? SelectedButtonView
        {
            get
            {
                if (m_selectedButton is null) return null;
                return ButtonViews.FirstOrDefault(b => b.Id == m_selectedButton.Id);
            }
            set
            {
                SetSelectedButton(value);
            }
        }

        public bool IsSelectedButtonAvailable => SelectedButton is not null;

        public string CopyName
        {
            get => m_copyName;
            set
            {
                m_copyName = value;
                OnPropertyChanged();
                IsCopyEnabled = !string.IsNullOrEmpty(m_copyName) && SelectedButtonName != m_copyName;
            }
        }

        public bool IsCopyEnabled
        {
            get => m_isCopyEnabled;
            set
            {
                m_isCopyEnabled = value;
                OnPropertyChanged();
            }
        }

        public string SelectedButtonName
        {
            get => SelectedButton?.Name ?? string.Empty;
            set
            {
                if (SelectedButton is not null && !string.IsNullOrWhiteSpace(value))
                {
                    SelectedButton.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool SelectedButtonVisible
        {
            get => SelectedButton?.Visible ?? false;
            set
            {
                if (SelectedButton is not null)
                {
                    SelectedButton.Visible = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedButtonPositionX
        {
            get => SelectedButton?.Position.X ?? -1;
            set
            {
                if (SelectedButton is not null)
                {
                    SelectedButton.Position = new Vector2(value, SelectedButton.Position.Y);
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedButtonPositionY
        {
            get => SelectedButton?.Position.Y ?? -1;
            set
            {
                if (SelectedButton is not null)
                {
                    SelectedButton.Position = new Vector2(SelectedButton.Position.X, value);
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedButtonRotation
        {
            get => SelectedButton?.Rotation ?? -1;
            set
            {
                if (SelectedButton is not null)
                {
                    SelectedButton.Rotation = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedButtonScaleX
        {
            get => SelectedButton?.Scale.X ?? -1;
            set
            {
                if (SelectedButton is not null)
                {
                    SelectedButton.Scale = new Vector2(value, SelectedButton.Scale.Y);
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedButtonScaleY
        {
            get => SelectedButton?.Scale.Y ?? -1;
            set
            {
                if (SelectedButton is not null)
                {
                    SelectedButton.Scale = new Vector2(SelectedButton.Scale.X, value);
                    OnPropertyChanged();
                }
            }
        }

        public int SelectedButtonZIndex
        {
            get => SelectedButton?.ZIndex ?? -1;
            set
            {
                if (SelectedButton is not null)
                {
                    SelectedButton.ZIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedButtonWidth
        {
            get => SelectedButton?.Width ?? -1;
            set
            {
                if (SelectedButton is not null)
                {
                    SelectedButton.Width = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedButtonHeight
        {
            get => SelectedButton?.Height ?? -1;
            set
            {
                if (SelectedButton is not null)
                {
                    SelectedButton.Height = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedButtonText
        {
            get => SelectedButton?.TextProperties.Text ?? string.Empty;
            set
            {
                if (SelectedButton is not null)
                {
                    SelectedButton.TextProperties.Text = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedButtonFontSize
        {
            get => SelectedButton?.TextProperties.FontSize ?? 24f;
            set
            {
                if (SelectedButton is not null)
                {
                    SelectedButton.TextProperties.FontSize = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedButtonTextColorHex
        {
            get => SelectedButton?.TextProperties.TextColorHex ?? "#FFFFFFFF";
            set
            {
                if (SelectedButton is not null)
                {
                    try
                    {
                        SelectedButton.TextProperties.TextColor = SKColor.Parse(value);
                        OnPropertyChanged();
                    }
                    catch
                    {
                        // Ignore invalid color strings
                    }
                }
            }
        }

        public ButtonContentsViewModel(IEngine engine)
        {
            m_engine = engine;
            ButtonViews = new ObservableCollection<ButtonListViewModel>();
            SelectedLayer = null;
            SelectedButton = null;
        }

        public void AddButtonToLayer(string name, string buttonText, float fontSize, bool visible, int width, int height, int backgroundTextureId)
        {
            if (m_engine.SceneManager.CurrentScene is null || SelectedLayer is null)
            {
                return;
            }

            ButtonObject button = new ButtonObject()
            {
                Name = name,
                TextProperties = new TextProperties()
                {
                    Text = buttonText,
                    FontSize = fontSize,
                    TextColor = SKColors.White,
                },
                Position = new Vector2(0, 0),
                BackgroundTextureId = backgroundTextureId,
                Visible = visible,
                Width = width,
                Height = height,
                Scale = new Vector2(1, 1),
            };

            m_engine.ObjectManager.AddSceneObject(m_engine.SceneManager.CurrentScene, SelectedLayer, button);
            ButtonViews.Add(new ButtonListViewModel(button));
            SelectedButton = button;
        }

        public void SetSelectedButton(ButtonListViewModel? buttonListViewModel)
        {
            if (m_selectedLayer is null || buttonListViewModel is null)
            {
                return;
            }

            SelectedButton = m_selectedLayer.ButtonObjects.FirstOrDefault(button => button.Id == buttonListViewModel.Id);
        }

        public void DeleteSelectedButton()
        {
            if (m_selectedLayer is null || SelectedButton is null)
            {
                return;
            }

            ButtonListViewModel? buttonListViewModel = ButtonViews.FirstOrDefault(button => button.Id == SelectedButton.Id);
            if (buttonListViewModel is not null)
            {
                ButtonViews.Remove(buttonListViewModel);
            }

            m_engine.ObjectManager.RemoveInteractiveObject(m_selectedLayer, SelectedButton);
            SelectedButton = null;
        }

        public void CopySelectedButton()
        {
            if (m_engine.SceneManager.CurrentScene is null || SelectedLayer is null || m_selectedButton is null)
            {
                return;
            }

            ButtonObject button = m_engine.ObjectManager.CopyButtonObject(m_engine.SceneManager.CurrentScene, SelectedLayer, m_selectedButton, CopyName);
            ButtonViews.Add(new ButtonListViewModel(button));
            SelectedButton = button;
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private ButtonObject? m_selectedButton;
        private GraphicsLayer? m_selectedLayer;
        private readonly IEngine m_engine;
        private string m_copyName = string.Empty;
        private bool m_isCopyEnabled;
    }
}
