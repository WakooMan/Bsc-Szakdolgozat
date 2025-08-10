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
    }
}
