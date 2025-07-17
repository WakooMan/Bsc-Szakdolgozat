using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Elements.Effects
{
    public class NewTurn : Effect
    {
        public NewTurn() { }
        public override void Apply()
        {
            throw new NotImplementedException();
        }

        public override Effect Clone()
        {
            return new NewTurn();
        }
    }
}
