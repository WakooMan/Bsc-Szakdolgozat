namespace GameLogic.Elements.Goods.Products
{
    public class Papirus : Product
    {
        public Papirus() :base() { }

        private Papirus(Papirus papirus) : base(papirus)
        {
        }

        public override Papirus Clone()
        {
            return new Papirus(this);
        }
    }
}
