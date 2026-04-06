using GameLogic.Elements;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using System;

namespace GameLogic.Handlers
{
    public class PlayerActionHandler: IPlayerActionHandler
    {
        public PlayerActionHandler() { }

        public async Task<IPlayerAction?> HandlePlayerActionsCompleted(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions)
        {
            bool completed = false;

            while (!completed)
            {
                var wrappers = await Task.WhenAll(playerActions.Select(async playerAction =>
                    new PlayerActionWrapper(playerAction, await playerAction.CanPerform(gameContext))));

                PlayerActionWrapper playerActionWrapper = gameContext.PlayerActionReceiver.ReceivePlayerAction(player, wrappers);
                if (playerActionWrapper.CanPerform)
                {
                    completed = await playerActionWrapper.PlayerAction.DoPlayerAction(gameContext);
                    return playerActionWrapper.PlayerAction;
                }
            }

            return null;
        }

        public async Task<(bool completed, IPlayerAction? playerAction)> HandlePlayerActions(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions)
        {
            var wrappers = await Task.WhenAll(playerActions.Select(async playerAction =>
                new PlayerActionWrapper(playerAction, await playerAction.CanPerform(gameContext))));

            PlayerActionWrapper playerActionWrapper = gameContext.PlayerActionReceiver.ReceivePlayerAction(player, wrappers);
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
