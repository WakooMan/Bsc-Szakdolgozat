using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.GameStructures;

namespace GameLogic.PlayerActions
{
    public class BuildWonder : IPlayerAction
    {

        public BuildWonder(IGameContext gameContext, Wonder wonder)
        {
            m_gameContext = gameContext;
            m_wonder = wonder;
        }

        public void DoPlayerAction()
        {
            if (Player.PickedCard is null)
            {
                throw new InvalidOperationException($"{Player.Name} player's picked card is null, {nameof(BuildWonder)} action cannot be performed!");
            }

            Composition.RemoveCard(Player.PickedCard);
            Player.Money -= m_gameContext.CostCalculator.GetBuildCost(m_wonder, Player, Opponent);
            m_wonder.HasBeenBuilt = true;
            m_gameContext.EventManager.Publish(GameEventType.WonderBuilt, new OnWonderBuilt(Player, m_wonder));
            m_wonder.OnBuilt(m_gameContext);
        }

        public bool CanPerform()
        {
            if (!Player.Wonders.Contains(m_wonder) || m_wonder.HasBeenBuilt || Player.PickedCard is null)
            {
                return false;
            }

            return m_gameContext.CostCalculator.CanAfford(m_wonder, Player, Opponent);
        }

        private ICardComposition Composition => m_gameContext.AgeHandler.CurrentAge.Composition;
        private Player Player => m_gameContext.TurnHandler.CurrentPlayer;
        private Player Opponent => m_gameContext.TurnHandler.OpponentPlayer;
        private readonly IGameContext m_gameContext;
        private readonly Wonder m_wonder;

    }
}
