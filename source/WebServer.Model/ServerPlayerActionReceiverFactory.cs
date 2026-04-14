using GameLogic.Interfaces;
using WebServer.Model.Client;

namespace WebServer.Model
{
    public class ServerPlayerActionReceiverFactory : IServerPlayerActionReceiverFactory
    {
        public IPlayerActionReceiver Create(IPlayerClient player)
        {
            return new ServerPlayerActionReceiver(player);
        }
    }
}
