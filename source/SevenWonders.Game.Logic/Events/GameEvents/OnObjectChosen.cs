namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnObjectChosen: GameEvent
    {
        public List<string> Objects { get; }
        public bool Visible { get; }
        public OnObjectChosen(ICollection<string> objects, bool visible)
        {
            Objects = objects.ToList();
            Visible = visible;
        }
    }
}
