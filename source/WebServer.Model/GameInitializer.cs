using GameLogic;
using Microsoft.Extensions.DependencyInjection;
using WebServer.Model.Client;
using WebServer.Model.PlayerStates;

namespace WebServer.Model
{
    public class GameInitializer: IGameInitializer
    {
        public GameInitializer(IServiceProvider serviceProvider)
        {
            m_serviceProvider = serviceProvider;
        }

        public IGame CreateAndInitialize(IPlayerClient player1, IPlayerClient player2, InGame player1State, InGame player2State)
        {
            var game = m_serviceProvider.GetRequiredService<IGame>();
            game.Initialize((player1.ApplicationUser.UserName, player1State), (player2.ApplicationUser.UserName, player2State));
            return game;
        }

        private readonly IServiceProvider m_serviceProvider;
    }
}
