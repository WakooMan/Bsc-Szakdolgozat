using SevenWonders.Game.Engine;

namespace SevenWonders.Game.Scene.Editor.ViewModels
{
    public class SceneTextureListViewModel : BaseListViewModel
    {
        public SceneTextureListViewModel(Texture texture)
        {
            m_texture = texture;
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Id));
        }

        public override string Name => m_texture.FileName;

        public override int Id => m_texture.Id;

        private readonly Texture m_texture;
    }
}
