using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Game.Engine.Components
{
    public interface IComponent
    {
        int Id { get; set; }
        string Name { get; set; }
        void Startup();
        void Update(float deltaTime);
        void Shutdown();
    }
}
