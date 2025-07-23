using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class NewTurn : Effect
    {
        public NewTurn() { }
        public override void Apply(PlayingState game)
        {
            throw new NotImplementedException();
        }

        public override NewTurn Clone()
        {
            return new NewTurn();
        }
    }
}
