namespace GameLogic.Handlers
{
    public interface IRandomElementReceiver
    {
        ICollection<T> TryReceiveRandomElements<T>(ICollection<T> elements, int num);
        ICollection<T> ReceiveRandomElements<T>(ICollection<T> elements, int num);
    }
}
