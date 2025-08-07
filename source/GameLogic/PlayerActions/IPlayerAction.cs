using GameLogic.Elements;

namespace GameLogic.PlayerActions
{
    public interface IPlayerAction
    {
        void DoPlayerAction(IGameContext gameContext);

        bool CanPerform(IGameContext gameContext);
    }
}
