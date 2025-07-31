namespace GameLogic.Elements.Goods.Products
{
    public class Glass : Product
    {
        private Glass(Glass glass) : base(glass)
        {
        }
        public Glass() : base() { }

        public override Glass Clone()
        {
            return new Glass(this);
        }

        public override bool Equals(Good? other)
        {
            if (other is Glass && Amount == other.Amount)
            {
                return true;
            }

            return false;
        }
    }
}
