using SevenWonders.GameEngine;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SevenWonders.SceneEditor.ViewModels
{
    public class TextLabelContentsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<TextLabelListViewModel> TextLabelViews { get; set; }

        public GraphicsLayer? SelectedLayer
        {
            get => m_selectedLayer;
            set
            {
                if (m_selectedLayer != value)
                {
                    m_selectedLayer = value;
                    SelectedTextLabel = null;
                    if (m_selectedLayer is not null)
                    {
                        TextLabelViews.Clear();
                        foreach (TextLabelListViewModel vm in m_selectedLayer.TextLabels.Select(label => new TextLabelListViewModel(label)))
                        {
                            TextLabelViews.Add(vm);
                        }
                    }
                }
            }
        }

        public TextLabel? SelectedTextLabel
        {
            get => m_selectedTextLabel;
            set
            {
                m_selectedTextLabel = value;
                OnPropertyChanged(nameof(IsSelectedTextLabelAvailable));
                OnPropertyChanged(nameof(SelectedTextLabelName));
                OnPropertyChanged(nameof(SelectedTextLabelVisible));
                OnPropertyChanged(nameof(SelectedTextLabelPositionX));
                OnPropertyChanged(nameof(SelectedTextLabelPositionY));
                OnPropertyChanged(nameof(SelectedTextLabelRotation));
                OnPropertyChanged(nameof(SelectedTextLabelScaleX));
                OnPropertyChanged(nameof(SelectedTextLabelScaleY));
                OnPropertyChanged(nameof(SelectedTextLabelZIndex));
                OnPropertyChanged(nameof(SelectedTextLabelWidth));
                OnPropertyChanged(nameof(SelectedTextLabelHeight));
                OnPropertyChanged(nameof(SelectedTextLabelText));
                OnPropertyChanged(nameof(SelectedTextLabelFontSize));
                OnPropertyChanged(nameof(SelectedTextLabelTextColorHex));
                OnPropertyChanged(nameof(SelectedTextLabelView));
            }
        }

        public TextLabelListViewModel? SelectedTextLabelView
        {
            get
            {
                if (m_selectedTextLabel is null) return null;
                return TextLabelViews.FirstOrDefault(t => t.Id == m_selectedTextLabel.Id);
            }
            set => SetSelectedTextLabel(value);
        }

        public bool IsSelectedTextLabelAvailable => SelectedTextLabel is not null;

        public string CopyName
        {
            get => m_copyName;
            set
            {
                m_copyName = value;
                OnPropertyChanged();
                IsCopyEnabled = !string.IsNullOrEmpty(m_copyName) && SelectedTextLabelName != m_copyName;
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

        public string SelectedTextLabelName
        {
            get => SelectedTextLabel?.Name ?? string.Empty;
            set
            {
                if (SelectedTextLabel is not null && !string.IsNullOrWhiteSpace(value))
                {
                    SelectedTextLabel.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool SelectedTextLabelVisible
        {
            get => SelectedTextLabel?.Visible ?? false;
            set
            {
                if (SelectedTextLabel is not null)
                {
                    SelectedTextLabel.Visible = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextLabelPositionX
        {
            get => SelectedTextLabel?.Position.X ?? -1;
            set
            {
                if (SelectedTextLabel is not null)
                {
                    SelectedTextLabel.Position = new Vector2(value, SelectedTextLabel.Position.Y);
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextLabelPositionY
        {
            get => SelectedTextLabel?.Position.Y ?? -1;
            set
            {
                if (SelectedTextLabel is not null)
                {
                    SelectedTextLabel.Position = new Vector2(SelectedTextLabel.Position.X, value);
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextLabelRotation
        {
            get => SelectedTextLabel?.Rotation ?? 0;
            set
            {
                if (SelectedTextLabel is not null)
                {
                    SelectedTextLabel.Rotation = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextLabelScaleX
        {
            get => SelectedTextLabel?.Scale.X ?? 1;
            set
            {
                if (SelectedTextLabel is not null)
                {
                    SelectedTextLabel.Scale = new Vector2(value, SelectedTextLabel.Scale.Y);
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextLabelScaleY
        {
            get => SelectedTextLabel?.Scale.Y ?? 1;
            set
            {
                if (SelectedTextLabel is not null)
                {
                    SelectedTextLabel.Scale = new Vector2(SelectedTextLabel.Scale.X, value);
                    OnPropertyChanged();
                }
            }
        }

        public int SelectedTextLabelZIndex
        {
            get => SelectedTextLabel?.ZIndex ?? 0;
            set
            {
                if (SelectedTextLabel is not null)
                {
                    SelectedTextLabel.ZIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextLabelWidth
        {
            get => SelectedTextLabel?.Width ?? -1;
            set
            {
                if (SelectedTextLabel is not null)
                {
                    SelectedTextLabel.Width = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextLabelHeight
        {
            get => SelectedTextLabel?.Height ?? -1;
            set
            {
                if (SelectedTextLabel is not null)
                {
                    SelectedTextLabel.Height = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedTextLabelText
        {
            get => SelectedTextLabel?.Text ?? string.Empty;
            set
            {
                if (SelectedTextLabel is not null)
                {
                    SelectedTextLabel.Text = value;
                    OnPropertyChanged();
                }
            }
        }

        public float SelectedTextLabelFontSize
        {
            get => SelectedTextLabel?.FontSize ?? 24f;
            set
            {
                if (SelectedTextLabel is not null)
                {
                    SelectedTextLabel.FontSize = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedTextLabelTextColorHex
        {
            get => SelectedTextLabel?.TextColorHex ?? "#FFFFFFFF";
            set
            {
                if (SelectedTextLabel is not null)
                {
                    try
                    {
                        SelectedTextLabel.TextColor = SKColor.Parse(value);
                        OnPropertyChanged();
                    }
                    catch
                    {
                        // Ignore invalid color strings
                    }
                }
            }
        }

        public TextLabelContentsViewModel(IEngine engine)
        {
            m_engine = engine;
            TextLabelViews = new ObservableCollection<TextLabelListViewModel>();
            SelectedLayer = null;
            SelectedTextLabel = null;
        }

        public void AddTextLabelToLayer(string name, string text, float fontSize, bool visible, int width, int height, int backgroundTextureId)
        {
            if (m_engine.SceneManager.CurrentScene is null || SelectedLayer is null)
            {
                return;
            }

            TextLabel textLabel = new TextLabel()
            {
                Name = name,
                Text = text,
                FontSize = fontSize,
                Position = new Vector2(0, 0),
                BackgroundTextureId = backgroundTextureId,
                Visible = visible,
                Width = width,
                Height = height,
                Scale = new Vector2(1, 1),
                TextColor = SkiaSharp.SKColors.White,
            };

            m_engine.ObjectManager.AddTextLabel(m_engine.SceneManager.CurrentScene, SelectedLayer, textLabel);
            TextLabelViews.Add(new TextLabelListViewModel(textLabel));
            SelectedTextLabel = textLabel;
        }

        public void SetSelectedTextLabel(TextLabelListViewModel? vm)
        {
            if (m_selectedLayer is null || vm is null)
            {
                return;
            }

            SelectedTextLabel = m_selectedLayer.TextLabels.FirstOrDefault(label => label.Id == vm.Id);
        }

        public void DeleteSelectedTextLabel()
        {
            if (m_selectedLayer is null || SelectedTextLabel is null)
            {
                return;
            }

            TextLabelListViewModel? vm = TextLabelViews.FirstOrDefault(t => t.Id == SelectedTextLabel.Id);
            if (vm is not null)
            {
                TextLabelViews.Remove(vm);
            }

            m_selectedLayer.TextLabels.Remove(SelectedTextLabel);
            SelectedTextLabel = null;
        }

        public void CopySelectedTextLabel()
        {
            if (m_engine.SceneManager.CurrentScene is null || SelectedLayer is null || m_selectedTextLabel is null)
            {
                return;
            }

            TextLabel textLabel = m_engine.ObjectManager.CopyTextLabel(m_engine.SceneManager.CurrentScene, SelectedLayer, m_selectedTextLabel, CopyName);
            TextLabelViews.Add(new TextLabelListViewModel(textLabel));
            SelectedTextLabel = textLabel;
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private TextLabel? m_selectedTextLabel;
        private GraphicsLayer? m_selectedLayer;
        private readonly IEngine m_engine;
        private string m_copyName = string.Empty;
        private bool m_isCopyEnabled;
    }
}
