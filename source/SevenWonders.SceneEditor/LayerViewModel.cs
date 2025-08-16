using SevenWonders.GameEngine;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SevenWonders.SceneEditor
{
    public class LayerViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public LayerViewModel(GraphicsLayer graphicsLayer)
        {
            m_graphicsLayer = graphicsLayer;

            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Id));
        }

        public string Name => m_graphicsLayer.Name;
        public int Id => m_graphicsLayer.ID;

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly GraphicsLayer m_graphicsLayer;
    }
}
