using System.Xml.Serialization;

namespace GameLogic.CardActions
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
