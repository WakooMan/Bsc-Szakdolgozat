using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Presenter.Connectors.Wonders;

namespace SevenWonders.Game.Presenter.GameEvents
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
