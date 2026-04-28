using GameLogic.Elements;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using SevenWonders.Common;

namespace GameLogic.Handlers
{
    public class PlayerActionHandler: IPlayerActionHandler
    {
        public PlayerActionHandler() { }

        public IPlayerAction? HandlePlayerActionsCompleted(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions)
        {
            if(player.PlayerActionReceiver is null)
            {
                throw new InvalidOperationException($"Player {player.Name} does not have a PlayerActionReceiver assigned.");
            }

            GameLog.Info($"HandlePlayerActionsCompleted: Player={player.Name}, ActionCount={playerActions.Count}");
            bool completed = false;

            while (!completed)
            {
                var wrappers = playerActions.Select(playerAction =>
                    new PlayerActionWrapper(playerAction, playerAction.CanPerform(gameContext))).ToList();

                PlayerActionWrapper playerActionWrapper = player.PlayerActionReceiver.ReceivePlayerAction(player, wrappers);
                if (playerActionWrapper.CanPerform)
                {
                    GameLog.Info($"Player={player.Name} performing action: {playerActionWrapper.PlayerAction.GetType().Name}");
                    completed = playerActionWrapper.PlayerAction.DoPlayerAction(gameContext);
                    if (completed)
                    {
                        GameLog.Info($"Player={player.Name} action completed: {playerActionWrapper.PlayerAction.GetType().Name}");
                        return playerActionWrapper.PlayerAction;
                    }
                }
            }

            return null;
        }

        public (bool completed, IPlayerAction? playerAction) HandlePlayerActions(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions)
        {
            if (player.PlayerActionReceiver is null)
            {
                throw new InvalidOperationException($"Player {player.Name} does not have a PlayerActionReceiver assigned.");
            }

            GameLog.Info($"HandlePlayerActions: Player={player.Name}, ActionCount={playerActions.Count}");
            var wrappers = playerActions.Select(playerAction =>
                new PlayerActionWrapper(playerAction, playerAction.CanPerform(gameContext))).ToList();

            PlayerActionWrapper playerActionWrapper = player.PlayerActionReceiver.ReceivePlayerAction(player, wrappers);
            if (playerActionWrapper.CanPerform)
            {
                GameLog.Info($"Player={player.Name} performing action: {playerActionWrapper.PlayerAction.GetType().Name}");
                return (playerActionWrapper.PlayerAction.DoPlayerAction(gameContext), playerActionWrapper.PlayerAction);
            }

            return (false, null);
        }

        public bool HandlePlayerAction(IGameContext gameContext, Player player, IPlayerAction playerAction)
        {
            PlayerActionWrapper playerActionWrapper = new PlayerActionWrapper(playerAction, playerAction.CanPerform(gameContext));
            if (playerActionWrapper.CanPerform)
            {
                return playerActionWrapper.PlayerAction.DoPlayerAction(gameContext);
            }

            return false;
        }
    }
}
