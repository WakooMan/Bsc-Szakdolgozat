namespace SevenWonders.Game.Scene.Editor.ViewModels
{
    public class AddTextLabelPopupWindowViewModel : AddPopupWindowViewModel
    {
        public string LabelText
        {
            get => m_labelText;
            set
            {
                m_labelText = value;
                OnPropertyChanged();
            }
        }

        public float FontSize
        {
            get => m_fontSize;
            set
            {
                m_fontSize = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => m_width;
            set
            {
                m_width = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        public int Height
        {
            get => m_height;
            set
            {
                m_height = value;
                OnPropertyChanged();
                m_onAddCommand.ChangeCanExecute();
            }
        }

        protected override bool CanExecuteAdd()
        {
            return base.CanExecuteAdd() && m_width > 0 && m_height > 0;
        }

        public AddTextLabelPopupWindowViewModel() : base()
        {
            m_labelText = string.Empty;
            m_fontSize = 24f;
        }

        public override void Clear()
        {
            base.Clear();
            m_labelText = string.Empty;
            m_fontSize = 24f;
            m_width = 0;
            m_height = 0;
        }

        private string m_labelText;
        private float m_fontSize;
        private int m_width;
        private int m_height;
    }
}
