namespace GameLogic.Events.GameEvents
{
    public class OnChooseObjects: GameEvent
    {
        public List<string> Objects { get; }
        public string Title { get; }
        public bool Visible { get; }
        public OnChooseObjects(string title, ICollection<string> objects, bool visible)
        {
            Title = title;
            Objects = objects.ToList();
            Visible = visible;
        }
    }
}
