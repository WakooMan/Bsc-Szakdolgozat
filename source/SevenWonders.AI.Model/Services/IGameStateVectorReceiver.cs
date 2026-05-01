using SevenWonders.Game.Logic.Elements;

namespace SevenWonders.AI.Model.Services
{
    public interface IGameStateVectorReceiver
    {
        void Initialize();
        List<float> Receive(PlayerProperties player1, PlayerProperties player2, PhaseIndicator phaseIndicator);
    }
}
