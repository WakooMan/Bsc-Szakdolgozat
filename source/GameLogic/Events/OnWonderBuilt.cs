using GameLogic.Elements;
using GameLogic.Elements.Wonders;

namespace GameLogic.Events
{
    public class OnWonderBuilt: EventArgs
    {
        public Wonder Wonder { get; }
        public Player Builder { get; }

        public OnWonderBuilt(Player builder, Wonder wonder)
        {
            Builder = builder;
            Wonder = wonder;
        }
    }
}
