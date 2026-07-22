namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnObjectChosen: GameEvent
    {
        public string ChosenObject { get; }
        public OnObjectChosen(string chosenObject)
        {
            ChosenObject = chosenObject;
        }
    }
}
