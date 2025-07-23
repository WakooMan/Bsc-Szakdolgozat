namespace GameLogic.Elements.Goods.Resources
{
    public class Stone : GameResource
    {
        public override GameResource Clone()
        {
            return new Stone();
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
