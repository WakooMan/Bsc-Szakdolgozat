using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
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

        public override ICard Clone()
        {
            return new BrownCard(this);
        }
    }
}
