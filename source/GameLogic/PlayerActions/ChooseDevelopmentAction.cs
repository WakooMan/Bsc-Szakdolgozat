using GameLogic.Elements.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.PlayerActions
{
    public class ChooseDevelopmentAction : IPlayerAction
    {
        public Development Development { get; }

        public ChooseDevelopmentAction(Development development)
        {
            Development = development;
        }

        public bool CanPerform()
        {
            return true;
        }

        public void DoPlayerAction()
        {
            
        }
    }
}
