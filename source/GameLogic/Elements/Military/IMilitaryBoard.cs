using GameLogic.Elements.Modifiers;
using GameLogic.Events;

namespace GameLogic.Elements.Military
{
    public interface IMilitaryBoard
    {
        List<MilitaryField> Fields { get; }
        List<MilitaryCard> MilitaryCards { get; }
        List<Development> Developments { get; }
        void Initialize(ICollection<Player> players, ICollection<Development> developments, IEventManager eventManager);
    }
}