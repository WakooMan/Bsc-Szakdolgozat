using SevenWonders.Game.Engine.SceneObjects;

namespace SevenWonders.Game.Scene.Editor.ViewModels
{
    public class TextLabelListViewModel : BaseListViewModel
    {
        public TextLabelListViewModel(TextLabel textLabel)
        {
            m_textLabel = textLabel;
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Id));
        }

        public override string Name => m_textLabel.Name;

        public override int Id => m_textLabel.Id;

        private readonly TextLabel m_textLabel;
    }
}
