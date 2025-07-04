using GameLogic.CardActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.GameCards
{
    public class YellowCard : Card
    {
        public int Point { get; set; }
        public List<CardAction> CardActions { get; set; }

        public YellowCard() : base()
        {
            CardActions = new List<CardAction>();
        }
    }
}
