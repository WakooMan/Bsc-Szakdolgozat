using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameLogic.CardActions
{
    [XmlInclude(typeof(BuyGood)), XmlInclude(typeof(ChooseGood))]
    public abstract class CardAction
    {
    }
}
