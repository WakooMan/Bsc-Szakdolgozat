using SevenWonders.GameEngine;

namespace SevenWonders.Presenter.Connectors
{
    public interface IGameObjectReceiver
    {
        GameObject ReceiveGameObject(string name);
        ICollection<GameObject> ReceiveGameObjects(string name, int number);
    }
}
