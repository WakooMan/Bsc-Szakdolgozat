using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GameLogic.Goods.Resources;

namespace GameLogic.GameCards
{
    public class BrownCard : Card
    {
        public List<GameResource> ProducedResources { get; set; }
        public BrownCard() : base()
        {
            ProducedResources = new List<GameResource>();
        }
    }
}
