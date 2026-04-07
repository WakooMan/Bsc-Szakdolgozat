using GameLogic.Elements.Modifiers;
using GameLogic.Events.GameEvents;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace GameLogic.Elements.Effects
{
    public class ChooseDevelopment : Effect
    {
        public ChooseDevelopment()
        {

        }

        public override ChooseDevelopment Clone()
        {
            return new ChooseDevelopment();
        }

        public override async Task Apply(IGameContext gameContext)
        {
            List<Development> developments = gameContext.DevelopmentList?.Developments ?? throw new InvalidOperationException($"{nameof(gameContext.DevelopmentList)} cannot be null in IGameContext object with parameter name: {nameof(gameContext)}!");
            List<Development> selected = developments.OrderBy(_ => gameContext.RandomGenerator.Next()).Take(3).ToList();
            await gameContext.EventManager.PublishAsync(new OnChooseDevelopmentsBegin(developments));
            await gameContext.PlayerActionHandler.HandlePlayerActionsCompleted(gameContext, gameContext.TurnHandler.CurrentPlayer, selected.Select(dev => (IPlayerAction)new ChooseDevelopmentAction(gameContext.TurnHandler.CurrentPlayer, dev, developments)).ToList());
            await gameContext.EventManager.PublishAsync(new OnChooseDevelopmentsEnd(developments));
        }
    }
}
