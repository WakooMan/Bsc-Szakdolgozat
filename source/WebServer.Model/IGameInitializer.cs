using GameLogic;
using WebServer.Model.Client;
using WebServer.Model.PlayerStates;

namespace WebServer.Model
{
    public interface IGameInitializer
    {
        IGame CreateAndInitialize(IPlayerClient player1, IPlayerClient player2, InGame player1State, InGame player2State);
    }
}
