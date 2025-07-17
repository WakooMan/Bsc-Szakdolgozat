namespace GameLogic.Elements.Wonders
{
    public interface IWonderList
    {
        List<Wonder> Wonders { get; }

        IWonderList Clone();
    }
}
