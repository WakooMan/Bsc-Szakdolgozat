namespace SevenWonders.Common
{
    public interface IRandomGenerator
    {
        int Next();
        int Next(int min, int max);
        ICollection<T> TryReceiveRandomElements<T>(ICollection<T> elements, int num);
        ICollection<T> ReceiveRandomElements<T>(ICollection<T> elements, int num);

    }
}
