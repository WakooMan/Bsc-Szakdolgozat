using GameLogic.Disciplines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.GameCards
{
    public class GreenCard : Card
    {
        public Discipline Discipline { get; set; }
        public int Point { get; set; }
        public GreenCard() : base()
        { }
    }
}
