using System.Xml.Serialization;

namespace GameLogic.Elements.CardActions
{
    [XmlInclude(typeof(BuyGoods)),
     XmlInclude(typeof(ChooseGood)),
     XmlInclude(typeof(GetMoney)),
     XmlInclude(typeof(GetMoneyForCard)),
     XmlInclude(typeof(GetMoneyForWonders))]
    public abstract class CardAction
    {
    }
}
