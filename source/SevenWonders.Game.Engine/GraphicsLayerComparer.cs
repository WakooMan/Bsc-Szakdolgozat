using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Game.Engine
{
    public class GraphicsLayerComparer : IComparer<GraphicsLayer>
    {
        public int Compare(GraphicsLayer? x, GraphicsLayer? y)
        {
            if (x is null || y is null)
            {
                throw new ArgumentNullException("One of the argument is a null reference!");
            }

            return x.ZIndex.CompareTo(y.ZIndex);
        }
    }
}
