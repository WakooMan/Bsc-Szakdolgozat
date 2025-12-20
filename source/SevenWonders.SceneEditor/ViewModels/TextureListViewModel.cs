using SevenWonders.GameEngine;

namespace SevenWonders.SceneEditor.ViewModels
{
    public class TextureListViewModel : BaseListViewModel
    {
        public TextureListViewModel(TextureObject texture)
        {
            m_texture = texture;
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Id));
        }
        public override string Name => m_texture.Name;

        public override int Id => m_texture.Id;

        private readonly TextureObject m_texture;
    }
}
