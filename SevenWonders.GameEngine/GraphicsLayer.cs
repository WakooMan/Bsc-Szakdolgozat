using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class GraphicsLayer
    {
        public List<GameObject> ObjectList { get; set; }
        public List<Texture> Textures { get; set; }
        public  bool Visible { get; set; }
        public bool EnableCollision { get; set; }
        public Camera Camera { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public Scene ParentScene { get; set; }

        public GraphicsLayer()
        {
            ObjectList = new List<GameObject>();
            Textures = new List<Texture>();
            Name = string.Empty;
            Camera = new Camera();
            ParentScene = new Scene();
        }

        public GraphicsLayer(GraphicsLayer graphicsLayer)
        {

        }
    }
}