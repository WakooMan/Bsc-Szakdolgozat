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

        public override bool Equals(Good? other)
        {
            if (other is Stone)
            {
                return true;
            }

            return false;
        }
    }
}
