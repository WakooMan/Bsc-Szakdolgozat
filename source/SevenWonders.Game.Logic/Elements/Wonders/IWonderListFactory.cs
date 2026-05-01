using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Game.Logic.Elements.Wonders
{
    public interface IWonderListFactory
    {
        IWonderList Create();
    }
}
