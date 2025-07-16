using System.Reflection.Metadata.Ecma335;

namespace GameLogic.Elements.Guilds
{
    public class MagistrateGuild : Guild
    {
        public override Guild Clone()
        {
            return new MagistrateGuild();
        }
    }
}