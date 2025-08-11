using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class Scene
    {
        public List<GraphicsLayer> Layers { get; set; }
        public string Name { get; set; }
        public bool Visible { get; set; }
        public Camera SceneCamera { get; set; }
        public int Id { get; set; }

        public Scene()
        {
            Layers = new List<GraphicsLayer>();
            Name = string.Empty;
            SceneCamera = new Camera();
        }

        public Scene(Scene scene)
        {

        }
    }
}