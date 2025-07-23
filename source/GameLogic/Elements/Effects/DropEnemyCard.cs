using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class DropEnemyCard : Effect
    {
        public string CardType { get; set; }

        public DropEnemyCard()
        {

        }

        private DropEnemyCard(DropEnemyCard dropEnemyCard)
        {
            CardType = dropEnemyCard.CardType;
        }

        public override void Apply(PlayingState game)
        {
            throw new NotImplementedException();
        }

        public override Effect Clone()
        {
            return new DropEnemyCard(this);
        }
    }
}
