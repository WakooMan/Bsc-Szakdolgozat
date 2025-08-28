using SevenWonders.GameEngine;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SevenWonders.SceneEditor.ViewModels
{
    public class SpriteListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name => m_sprite.Name;

        public Sprite Sprite => m_sprite;

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
