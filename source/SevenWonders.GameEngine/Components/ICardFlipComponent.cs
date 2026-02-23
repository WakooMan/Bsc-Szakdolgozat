using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.GameEngine.Components
{
    public interface ICardFlipComponent: IComponent
    {
        void Flip(GameObject gameObject, int spriteNumber, float flipSpeed);
    }
}
