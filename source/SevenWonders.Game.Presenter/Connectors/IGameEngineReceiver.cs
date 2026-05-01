using SevenWonders.Game.Engine;

namespace SevenWonders.Game.Presenter.Connectors
{
    public interface IGameEngineReceiver
    {
        GameObject ReceiveGameObject(string name);
        TextLabel ReceiveTextLabel(string name);
        ICollection<TextLabel> ReceiveTextLabels(string name, int number);
        IInteractiveObject ReceiveInteractiveObject(string name);
        ICollection<GameObject> ReceiveGameObjects(string name, int number);
        GraphicsLayer ReceiveGraphicsLayer(string name);
        ButtonObject ReceiveButton(string name);
    }
}
