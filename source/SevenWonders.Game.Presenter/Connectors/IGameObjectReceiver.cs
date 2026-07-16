using SevenWonders.Game.Engine.SceneObjects;

namespace SevenWonders.Game.Presenter.Connectors
{
    public interface IGameObjectReceiver
    {
        GameObject ReceiveGameObject(string name);
        ICollection<GameObject> ReceiveGameObjects(string name, int number);
    }
}
