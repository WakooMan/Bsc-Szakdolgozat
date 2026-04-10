using GameLogic.Elements.Goods.Resources;
using GameLogic.Events.GameEvents;

namespace GameLogic.Elements.GameCards
{
    public class BrownCard : Card
    {
        public List<GameResource> ProducedResources { get; set; }
        public BrownCard() : base()
        {
            ProducedResources = new List<GameResource>();
        }

        private BrownCard(BrownCard brownCard) : base(brownCard)
        {
            ProducedResources = brownCard.ProducedResources.Select(res => res.Clone()).ToList();
        }

        public override BrownCard Clone()
        {
            return new BrownCard(this);
        }

        public override Task OnBuilt(IGameContext gameContext, int playerId)
        {
            gameContext.EventManager.Subscribe<OnCalculatePlayerProperties>(OnCalculatePlayerProperties);
            return Task.CompletedTask;
        }

        public override Task OnDestroyed(IGameContext gameContext, int playerId)
        {
            gameContext.EventManager.Unsubscribe<OnCalculatePlayerProperties>(OnCalculatePlayerProperties);
            return Task.CompletedTask;
        }

        private void OnCalculatePlayerProperties(OnCalculatePlayerProperties properties)
        {
            if (properties.PlayerProperties.Player.HasCard(this))
            {
                foreach (GameResource resource in ProducedResources)
                {
                    properties.PlayerProperties.AddGood(resource);
                }
            }
        }
    }
}
