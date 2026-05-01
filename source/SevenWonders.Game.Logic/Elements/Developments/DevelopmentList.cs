using SevenWonders.Game.Logic.Elements.Modifiers;
using SevenWonders.Game.Logic.Elements.Wonders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Game.Logic.Elements.Developments
{
    public class DevelopmentList: IDevelopmentList
    {
        public List<Development> Developments { get; set; }

        public DevelopmentList()
        {
            Developments = new List<Development>();
        }

        private DevelopmentList(DevelopmentList developmentList)
        {
            Developments = developmentList.Developments.Select(dev => dev.Clone()).ToList();
        }

        public IDevelopmentList Clone()
        {
            return new DevelopmentList(this);
        }
    }
}
