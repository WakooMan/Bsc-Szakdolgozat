namespace GameLogic.Elements.Effects
{
    public class BuyGoodItem
    {
        public int MoneyCost { get; set; }
        public string GoodType { get; set; }

        public BuyGoodItem()
        {
            GoodType = string.Empty;
        }

        public BuyGoodItem(BuyGoodItem buyGoodItem)
        {
            MoneyCost = buyGoodItem.MoneyCost;
            GoodType = buyGoodItem.GoodType;
        }
    }
}
