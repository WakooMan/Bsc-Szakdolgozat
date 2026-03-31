using GameLogic.PlayerActions;

namespace GameLogic.Interfaces
{
    public class PlayerActionWrapper
    {
        public IPlayerAction PlayerAction { get; }
        public bool CanPerform { get; }
        public PlayerActionWrapper(IPlayerAction playerAction, bool canPerform)
        {
            PlayerAction = playerAction;
            CanPerform = canPerform;
        }
    }
}
