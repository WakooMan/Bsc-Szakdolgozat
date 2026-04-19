using GameLogic.Elements.Modifiers;
using GameLogic.Events.GameEvents;
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

        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            List<Development> developments = gameContext.DevelopmentList?.Developments ?? throw new InvalidOperationException($"{nameof(gameContext.DevelopmentList)} cannot be null in IGameContext object with parameter name: {nameof(gameContext)}!");
            List<Development> selected = developments.OrderBy(_ => gameContext.RandomGenerator.Next()).Take(3).ToList();

            gameContext.EventManager.Publish(new OnChooseObjects("Válassz fejlesztést", selected.Select(dev => dev.Name).ToArray(), true));
            gameContext.PlayerActionHandler.HandlePlayerActions(gameContext,
                  owner, 
                  selected.Select(dev => (IPlayerAction)new ChooseDevelopmentAction(owner, opponent, dev, developments)).ToList());
        }
    }
}
