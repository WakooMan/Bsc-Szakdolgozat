namespace GameLogic.Events.GameEvents
{
    public class OnChooseObjects: GameEvent
    {
        public List<string> Objects { get; }
        public string Title { get; }
        public string? LayerName { get; }
        public OnChooseObjects(string title, ICollection<string> objects, string? layerName = null)
        {
            Title = title;
            Objects = objects.ToList();
            LayerName = layerName;
        }
    }
}
