using GameLogic.Elements;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace GameLogic.Handlers
{
    public class PlayerActionHandler: IPlayerActionHandler
    {
        public PlayerActionHandler() { }

        public async Task<IPlayerAction?> HandlePlayerActionsCompleted(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions)
        {
            if(player.PlayerActionReceiver is null)
            {
                throw new InvalidOperationException($"Player {player.Name} does not have a PlayerActionReceiver assigned.");
            }

            bool completed = false;

            while (!completed)
            {
                var wrappers = await Task.WhenAll(playerActions.Select(async playerAction =>
                    new PlayerActionWrapper(playerAction, await playerAction.CanPerform(gameContext))));

                PlayerActionWrapper playerActionWrapper = player.PlayerActionReceiver.ReceivePlayerAction(player, wrappers);
                if (playerActionWrapper.CanPerform)
                {
                    completed = await playerActionWrapper.PlayerAction.DoPlayerAction(gameContext);
                    if (completed)
                    {
                        return playerActionWrapper.PlayerAction;
                    }
                }
            }

            return null;
        }

        public async Task<(bool completed, IPlayerAction? playerAction)> HandlePlayerActions(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions)
        {
            if (player.PlayerActionReceiver is null)
            {
                throw new InvalidOperationException($"Player {player.Name} does not have a PlayerActionReceiver assigned.");
            }

            var wrappers = await Task.WhenAll(playerActions.Select(async playerAction =>
                new PlayerActionWrapper(playerAction, await playerAction.CanPerform(gameContext))));

            PlayerActionWrapper playerActionWrapper = player.PlayerActionReceiver.ReceivePlayerAction(player, wrappers);
            if (playerActionWrapper.CanPerform)
            {
                return (await playerActionWrapper.PlayerAction.DoPlayerAction(gameContext), playerActionWrapper.PlayerAction);
            }

            return (false, null);
        }

        public async Task<bool> HandlePlayerAction(IGameContext gameContext, Player player, IPlayerAction playerAction)
        {
            PlayerActionWrapper playerActionWrapper = new PlayerActionWrapper(playerAction, await playerAction.CanPerform(gameContext));
            if (playerActionWrapper.CanPerform)
            {
                return await playerActionWrapper.PlayerAction.DoPlayerAction(gameContext);
            }

            return false;
        }
    }
}
