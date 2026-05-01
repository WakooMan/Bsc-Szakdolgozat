using SevenWonders.Game.Engine;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SevenWonders.Game.Scene.Editor.ViewModels
{
    public class SpriteListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name => m_sprite.Name;

        public Sprite Sprite => m_sprite;

        public override string ToString() => Name;

        public SpriteListViewModel(Sprite sprite)
        {
            m_sprite = sprite;
            OnPropertyChanged(nameof(Name));
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private Sprite m_sprite;
    }
}
