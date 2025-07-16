namespace GameLogic.Elements.Wonders
{
    public interface IWonder
    {
        string Name { get; }

        IWonder Clone();

    }
}
