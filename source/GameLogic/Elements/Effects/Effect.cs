using System.Xml.Serialization;

namespace GameLogic.Elements.Effects
{
    [XmlInclude(typeof(BuyGoods)),
     XmlInclude(typeof(ChooseGood)),
     XmlInclude(typeof(GetMoney)),
     XmlInclude(typeof(GetMoneyForCard)),
     XmlInclude(typeof(GetMoneyForWonders))]
    public abstract class Effect
    {
        public abstract Effect Clone();
        public abstract void Apply();
    }
}
