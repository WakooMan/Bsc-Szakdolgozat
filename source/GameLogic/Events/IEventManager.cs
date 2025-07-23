using GameLogic.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Events
{
    public interface IEventManager
    {
        void Subscribe(GameEventType eventType, Action<EventArgs> listener);
        void Publish(GameEventType eventType, EventArgs game);
    }
}
