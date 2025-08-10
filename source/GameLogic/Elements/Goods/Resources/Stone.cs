namespace GameLogic.Elements.Goods.Resources
{
    public class Stone : GameResource
    {
        public Stone(): base() { }

        private Stone(Stone stone):base(stone){}
        public override Stone Clone()
        {
            return new Stone(this);
        }
    }
}
