using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class ChooseDevelopment : Effect
    {
        public ChooseDevelopment()
        {

        }

        public override Effect Clone()
        {
            return new ChooseDevelopment();
        }
    }
}
