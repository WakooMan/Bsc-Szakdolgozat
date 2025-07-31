namespace GameLogic.Elements.Goods.Resources
{
    public class Wood : GameResource
    {
        public override Wood Clone()
        {
            return new Wood();
        }

        public override bool Equals(Good? other)
        {
            if (other is Wood)
            {
                return true;
            }

            return false;
        }
    }
}
