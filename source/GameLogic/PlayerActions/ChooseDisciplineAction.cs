using GameLogic.Elements.Disciplines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.PlayerActions
{
    public class ChooseDisciplineAction: IPlayerAction
    {
        public Discipline Discipline { get; }

        public ChooseDisciplineAction(Discipline discipline)
        {
            Discipline = discipline;
        }

        public void DoPlayerAction()
        {
        }

        public bool CanPerform()
        {
            return true;
        }
    }
}
