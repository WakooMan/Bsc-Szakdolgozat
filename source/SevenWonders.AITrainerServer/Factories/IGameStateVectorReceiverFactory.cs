using SevenWonders.AI.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.AITrainerServer.Factories
{
    public interface IGameStateVectorReceiverFactory
    {
        IGameStateVectorReceiver Create();
    }
}
