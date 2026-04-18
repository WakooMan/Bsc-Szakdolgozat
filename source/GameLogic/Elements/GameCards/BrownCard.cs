using GameLogic.Elements.Goods.Resources;

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

        public override Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            foreach (GameResource resource in ProducedResources)
            {
                playerProperties.AddGood(resource);
            }
            return Task.CompletedTask;
        }
    }
}
