using GameLogic.Events.GameEvents;
using SevenWonders.Presenter.Connectors.Wonders;

namespace SevenWonders.Presenter.GameEvents
{
    public class OnCardBuiltIntoWonder : OnWonderBuilt
    {
        public WonderConnection WonderConnection { get; }

        public OnCardBuiltIntoWonder(OnWonderBuilt wonderBuilt, WonderConnection wonderConnection) : base(wonderBuilt.Builder, wonderBuilt.Card, wonderBuilt.Wonder)
        {
            WonderConnection = wonderConnection;
        }
    }
}
