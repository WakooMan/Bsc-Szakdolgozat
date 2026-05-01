using SevenWonders.Game.Logic.Elements.Modifiers;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.PlayerActions;

namespace SevenWonders.Game.Logic.Elements.Effects
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
            List<Development> selected = gameContext.RandomGenerator.TryReceiveRandomElements(developments, 3).ToList();

            if (selected.Count > 0)
            {
                gameContext.EventManager.Publish(new OnChooseObjects("Válassz fejlesztést", selected.Select(dev => dev.Name).ToArray()));
                gameContext.PlayerActionHandler.HandlePlayerActions(gameContext,
                      owner,
                      selected.Select(dev => (IPlayerAction)new ChooseDevelopmentAction(owner, opponent, dev, developments, true)).ToList());
            }
        }
    }
}
