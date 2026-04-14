using GameLogic;
using Microsoft.Extensions.DependencyInjection;
using WebServer.Model.Client;

namespace WebServer.Model
{
    public class GameInitializer: IGameInitializer
    {
        public GameInitializer(IServiceProvider serviceProvider, IServerPlayerActionReceiverFactory serverPlayerActionReceiverFactory)
        {
            m_serviceProvider = serviceProvider;
            m_serverPlayerActionReceiverFactory = serverPlayerActionReceiverFactory;
        }

        public IGame CreateAndInitialize(IPlayerClient player1, IPlayerClient player2)
        {
            var game = m_serviceProvider.GetRequiredService<IGame>();
            game.Initialize((player1.ApplicationUser.UserName, m_serverPlayerActionReceiverFactory.Create(player1)), (player2.ApplicationUser.UserName, m_serverPlayerActionReceiverFactory.Create(player2)));
            return game;
        }

        private readonly IServiceProvider m_serviceProvider;
        private readonly IServerPlayerActionReceiverFactory m_serverPlayerActionReceiverFactory;
    }
}
