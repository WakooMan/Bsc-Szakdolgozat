using SevenWonders.Game.Logic.Elements.Modifiers;

namespace SevenWonders.Game.Logic.Elements.Developments
{
    public interface IDevelopmentList
    {
        List<Development> Developments { get; }

        IDevelopmentList Clone();
    }
}