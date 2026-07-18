namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnObjectChosen: GameEvent
    {
        public List<string> Objects { get; }
        public OnObjectChosen(ICollection<string> objects)
        {
            Objects = objects.ToList();
        }
    }
}
