using GameLogic.Elements.Modifiers;
using GameLogic.PlayerActions;

namespace GameLogic.Elements.Effects
{
    public class ChooseDevelopment : Effect
    {
        public ChooseDevelopment()
        {

        }

        public override Effect Clone()
        {
            return new ChooseDevelopment();
        }

        public override void Apply(IGameContext gameContext)
        {
            List<Development> developments = gameContext.DevelopmentList.Developments;
            List<Development> selected = developments.OrderBy(_ => gameContext.RandomGenerator.Next()).Take(3).ToList();
            selected.ForEach(item => developments.Remove(item));
            ChooseDevelopmentAction chooseDevelopmentAction = gameContext.PlayerActionReceiver.ReceivePlayerAction<ChooseDevelopmentAction>(gameContext.TurnHandler.CurrentPlayer, selected.Select(dev => new ChooseDevelopmentAction(dev)).ToArray());
            gameContext.TurnHandler.CurrentPlayer.Developments.Add(chooseDevelopmentAction.Development);
            chooseDevelopmentAction.Development.OnDevelopmentEstablished(gameContext);
        }
    }
}
