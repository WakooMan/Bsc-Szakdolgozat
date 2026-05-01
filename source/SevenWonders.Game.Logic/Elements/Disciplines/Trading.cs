using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Game.Logic.Elements.Disciplines
{
    public class Trading : Discipline
    {
        public override Discipline Clone()
        {
            return new Trading();
        }
    }
}
