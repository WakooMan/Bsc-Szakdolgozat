using GameLogic.Elements.Disciplines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Elements.GameCards
{
    public class GreenCard : Card
    {
        public Discipline Discipline { get; set; }
        public int Point { get; set; }
        public GreenCard() : base()
        { }

        private GreenCard(GreenCard greenCard) : base(greenCard)
        {
            Discipline = greenCard.Discipline.Clone();
            Point = greenCard.Point;
        }

        public override ICard Clone()
        {
            return new GreenCard(this);
        }
    }
}
