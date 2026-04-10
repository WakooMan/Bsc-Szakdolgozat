using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.GameStates
{
    public interface IGameState
    {
        Task DoStateAction();
        IGameState GetNextState();
    }
}
