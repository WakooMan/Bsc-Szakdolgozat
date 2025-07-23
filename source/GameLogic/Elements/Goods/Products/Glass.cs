namespace GameLogic.Elements.Goods.Products
{
    public class Glass : Product
    {
        public override Product Clone()
        {
            return new Glass();
        }

        public override bool Equals(Good? other)
        {
            if (other is Glass)
            {
                return true;
            }

            return false;
        }
    }
}
