namespace GameLogic.Elements.Goods.Products
{
    public class Papirus : Product
    {
        public override Product Clone()
        {
            return new Papirus();
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
