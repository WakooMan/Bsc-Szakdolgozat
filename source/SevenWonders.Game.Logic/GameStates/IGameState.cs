using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Game.Logic.GameStates
{
    public interface IGameState
    {
        void DoStateAction();
        IGameState GetNextState();
    }
}
