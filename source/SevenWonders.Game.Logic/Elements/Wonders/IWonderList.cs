namespace SevenWonders.Game.Logic.Elements.Wonders
{
    public interface IWonderList
    {
        List<Wonder> Wonders { get; }

        IWonderList Clone();
    }
}
