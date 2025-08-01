namespace GameLogic.Elements.Goods.Resources
{
    public class Wood : GameResource
    {
        public Wood(): base() { }

        private Wood(Wood wood) : base(wood) { }

        public override Wood Clone()
        {
            return new Wood(this);
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
