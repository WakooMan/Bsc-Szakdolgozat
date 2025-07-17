using GameLogic.Elements.Modifiers;

namespace GameLogic.Elements.Developments
{
    public interface IDevelopmentList
    {
        List<Development> Developments { get; }

        IDevelopmentList Clone();
    }
}