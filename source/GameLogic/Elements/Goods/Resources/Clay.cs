namespace GameLogic.Elements.Goods.Resources
{
    public class Clay : GameResource
    {
        public override GameResource Clone()
        {
            return new Clay();
        }

        public override bool Equals(Good? other)
        {
            if (other is Clay)
            {
                return true;
            }

            return false;
        }
    }
}
