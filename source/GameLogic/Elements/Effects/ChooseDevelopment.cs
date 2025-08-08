using GameLogic.Elements.Modifiers;
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

        public override void Apply(IGameContext gameContext)
        {
            List<Development> developments = gameContext.DevelopmentList?.Developments ?? throw new InvalidOperationException($"{nameof(gameContext.DevelopmentList)} cannot be null in IGameContext object with parameter name: {nameof(gameContext)}!");
            List<Development> selected = developments.OrderBy(_ => gameContext.RandomGenerator.Next()).Take(3).ToList();
            IPlayerAction playerAction = gameContext.PlayerActionReceiver.ReceivePlayerAction(gameContext.TurnHandler.CurrentPlayer, selected.Select(dev => new ChooseDevelopmentAction(gameContext.TurnHandler.CurrentPlayer, dev, developments)).ToArray());
            if (playerAction.CanPerform(gameContext))
            {
                playerAction.DoPlayerAction(gameContext);
            }
        }
    }
}
