namespace GameLogic.Elements.Goods.Resources
{
    public class Clay : GameResource
    {
        public Clay(): base() { }

        private Clay(Clay clay):base(clay) { }

        public override Clay Clone()
        {
            return new Clay(this);
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
