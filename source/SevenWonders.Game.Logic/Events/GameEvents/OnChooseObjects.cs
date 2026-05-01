namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnChooseObjects: GameEvent
    {
        public List<string> Objects { get; }
        public string Title { get; }
        public OnChooseObjects(string title, ICollection<string> objects)
        {
            Title = title;
            Objects = objects.ToList();
        }
    }
}
