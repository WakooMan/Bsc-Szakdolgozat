namespace GameLogic.Events.GameEvents
{
    public class OnObjectChosen: GameEvent
    {
        public List<string> Objects { get; }
        public string? LayerName { get; }
        public OnObjectChosen(ICollection<string> objects, string? layerName = null)
        {
            Objects = objects.ToList();
            LayerName = layerName;
        }
    }
}
