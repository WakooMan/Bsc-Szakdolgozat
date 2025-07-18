using GameLogic.Elements;

namespace GameLogic.PlayerActions
{
    public interface IPlayerAction
    {
        void DoPlayerAction();

        bool CanPerform();
    }
}
