using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class BuildWonder : IPlayerAction
    {
        public string Name => m_wonder.Name;
        public Wonder Wonder => m_wonder;
        public BuildWonder(Wonder wonder)
        {
            m_wonder = wonder;
        }

        public async Task<bool> DoPlayerAction(IGameContext gameContext)
        {
            Player player = GetPlayer(gameContext);
            Player opponent = GetOpponent(gameContext);
            if (player.PickedCard is null)
            {
                throw new InvalidOperationException($"{player.Name} player's picked card is null, {nameof(BuildWonder)} action cannot be performed!");
            }
            ArgumentChecker.CheckPredicateForOperation(() => !player.Wonders.Contains(m_wonder) || m_wonder.HasBeenBuilt, "Player already built the wonder or he/she does not have this wonder.");

            GetComposition(gameContext).RemoveCard(player.PickedCard);
            player.Money -= await gameContext.CostCalculator.GetBuildCost(m_wonder, player, opponent);
            m_wonder.HasBeenBuilt = true;
            Card card = player.PickedCard.CardObj;
            player.PickedCard = null;
            await gameContext.EventManager.PublishAsync(new OnWonderBuilt(player, card, m_wonder));
            await m_wonder.OnBuilt(gameContext, player.Id);
            await gameContext.EventManager.PublishAsync(new AfterBuildableBuilt(player, opponent, m_wonder));
            return true;
        }

        public async Task<bool> CanPerform(IGameContext gameContext)
        {
            Player player = GetPlayer(gameContext);
            Player opponent = GetOpponent(gameContext);
            if (!player.Wonders.Contains(m_wonder) || m_wonder.HasBeenBuilt || player.PickedCard is null)
            {
                return false;
            }

            return await gameContext.CostCalculator.CanAfford(m_wonder, player, opponent);
        }

        private ICardComposition GetComposition(IGameContext gameContext) => gameContext.AgeHandler.CurrentAge.Composition;
        private Player GetPlayer(IGameContext gameContext) => gameContext.TurnHandler.CurrentPlayer;
        private Player GetOpponent(IGameContext gameContext) => gameContext.TurnHandler.OpponentPlayer;
        private readonly Wonder m_wonder;

    }
}
