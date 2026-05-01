using SevenWonders.Game.Logic.PlayerActions;

namespace SevenWonders.Game.Logic.Interfaces
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
