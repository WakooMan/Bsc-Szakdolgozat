using GameLogic.Elements.Modifiers;

namespace GameLogic.Elements.Military
{
    public interface IMilitaryBoard
    {
        List<MilitaryField> Fields { get; }
        List<MilitaryCard> MilitaryCards { get; }
        List<Development> Developments { get; }
        void Initialize(ICollection<Player> players, ICollection<Development> developments);
        void OnUpdate(IGameContext gameContext, PlayerProperties player1, PlayerProperties player2);
    }
}