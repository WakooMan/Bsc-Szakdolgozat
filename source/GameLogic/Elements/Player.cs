using GameLogic.Elements.Wonders;
using GameLogic.PlayerActions;

namespace GameLogic.Elements
{
    public class Player
    {
        public string Name { get; set; }
        public List<Wonder> Wonders { get; }

        public Player(string name)
        {
            Name = name;
            Wonders = new List<Wonder>();
        }
    }
}
