using GameLogic.Elements.GameCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.GameStructures.Factories
{
    public interface ICardNodeFactory
    {
        public ICardNode Create(Card card);
    }
}
