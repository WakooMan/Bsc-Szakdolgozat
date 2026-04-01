using GameLogic.Elements;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using System;

namespace GameLogic.Handlers
{
    public class PlayerActionHandler: IPlayerActionHandler
    {
        public PlayerActionHandler() { }

        public async Task HandlePlayerActionsCompleted(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions)
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
                }
            }
        }

        public async Task<bool> HandlePlayerActions(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions)
        {
            var wrappers = await Task.WhenAll(playerActions.Select(async playerAction =>
                new PlayerActionWrapper(playerAction, await playerAction.CanPerform(gameContext))));

            PlayerActionWrapper playerActionWrapper = gameContext.PlayerActionReceiver.ReceivePlayerAction(player, wrappers);
            if (playerActionWrapper.CanPerform)
            {
                return await playerActionWrapper.PlayerAction.DoPlayerAction(gameContext);
            }

            return false;
        }

        public async Task HandlePlayerAction(IGameContext gameContext, Player player, IPlayerAction playerAction)
        {
            PlayerActionWrapper playerActionWrapper = new PlayerActionWrapper(playerAction, await playerAction.CanPerform(gameContext));
            if (playerActionWrapper.CanPerform)
            {
                await playerActionWrapper.PlayerAction.DoPlayerAction(gameContext);
            }
        }
    }
}
