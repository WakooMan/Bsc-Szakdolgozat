using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.GameStructures;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.PlayerActions
{
    public class BuildWonder : IPlayerAction
    {
        public string Name => m_wonder.Name;
        public int Id => 4;
        public Wonder Wonder => m_wonder;
        public BuildWonder(Wonder wonder)
        {
            m_wonder = wonder;
        }

        public bool DoPlayerAction(IGameContext gameContext)
        {
            Player player = GetPlayer(gameContext);
            Player opponent = GetOpponent(gameContext);
            if (player.PickedCard is null)
            {
                throw new InvalidOperationException($"{player.Name} player's picked card is null, {nameof(BuildWonder)} action cannot be performed!");
            }
            ArgumentChecker.CheckPredicateForOperation(() => !player.Wonders.Contains(m_wonder) || m_wonder.HasBeenBuilt, "Player already built the wonder or he/she does not have this wonder.");
            int totalBuiltWonders = player.Wonders.Count(w => w.HasBeenBuilt) + opponent.Wonders.Count(w => w.HasBeenBuilt);
            ArgumentChecker.CheckPredicateForOperation(() => totalBuiltWonders >= 7, "Cannot build more than 7 wonders overall.");

            GetComposition(gameContext).RemoveCard(player.PickedCard);
            player.Money -= gameContext.CostCalculator.GetBuildCost(m_wonder, player, opponent);
            m_wonder.HasBeenBuilt = true;
            Card card = player.PickedCard.CardObj;
            player.PickedCard = null;
            OnWonderBuilt onWonderBuilt = new OnWonderBuilt(player, card, m_wonder);
            player.OnBuildWonder(onWonderBuilt);
            gameContext.EventManager.Publish(onWonderBuilt);
            m_wonder.OnBuilt(gameContext, player, opponent);
            return true;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            Player player = GetPlayer(gameContext);
            Player opponent = GetOpponent(gameContext);
            int totalBuiltWonders = player.Wonders.Count(w => w.HasBeenBuilt) + opponent.Wonders.Count(w => w.HasBeenBuilt);
            if (!player.Wonders.Contains(m_wonder) || m_wonder.HasBeenBuilt || player.PickedCard is null || totalBuiltWonders >= 7)
            {
                return false;
            }

            return gameContext.CostCalculator.CanAfford(m_wonder, player, opponent);
        }

        private ICardComposition GetComposition(IGameContext gameContext) => gameContext.AgeHandler.CurrentAge.Composition;
        private Player GetPlayer(IGameContext gameContext) => gameContext.TurnHandler.CurrentPlayer;
        private Player GetOpponent(IGameContext gameContext) => gameContext.TurnHandler.OpponentPlayer;
        private readonly Wonder m_wonder;

    }
}
