namespace SevenWonders.Common
{
    public interface IRandomGenerator
    {
        int Next();
        int Next(int min, int max);
    }
}
