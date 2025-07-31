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

        public override bool Equals(Good? other)
        {
            if (other is Papirus)
            {
                return true;
            }

            return false;
        }
    }
}
