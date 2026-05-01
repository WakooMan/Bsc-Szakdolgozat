using SevenWonders.Game.Engine;

namespace SevenWonders.Game.Scene.Editor.ViewModels
{
    public class ButtonListViewModel : BaseListViewModel
    {
        public ButtonListViewModel(ButtonObject button)
        {
            m_button = button;
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Id));
        }

        public override string Name => m_button.Name;

        public override int Id => m_button.Id;

        private readonly ButtonObject m_button;
    }
}
