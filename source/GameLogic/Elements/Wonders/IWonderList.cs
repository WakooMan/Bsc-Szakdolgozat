namespace GameLogic.Elements.Wonders
{
    public interface IWonderList
    {
        List<IWonder> Wonders { get; }

        IWonderList Clone();
    }
}
