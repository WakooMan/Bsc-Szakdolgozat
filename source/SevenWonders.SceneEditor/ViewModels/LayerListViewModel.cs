using SevenWonders.GameEngine;

namespace SevenWonders.SceneEditor.ViewModels
{
    public class LayerListViewModel: BaseListViewModel
    {

        public LayerListViewModel(GraphicsLayer graphicsLayer)
        {
            m_graphicsLayer = graphicsLayer;
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Id));
        }

        public override string Name => m_graphicsLayer.Name;
        public override int Id => m_graphicsLayer.ID;

        private readonly GraphicsLayer m_graphicsLayer;
    }
}
