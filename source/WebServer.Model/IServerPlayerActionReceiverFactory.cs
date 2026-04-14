using GameLogic.Interfaces;
using WebServer.Model.Client;

namespace WebServer.Model
{
    public interface IServerPlayerActionReceiverFactory
    {
        IPlayerActionReceiver Create(IPlayerClient player);
    }
}
