using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Elements.Disciplines
{
    public class Mechanics : Discipline
    {
        public override Discipline Clone()
        {
            return new Mechanics();
        }
    }
}
