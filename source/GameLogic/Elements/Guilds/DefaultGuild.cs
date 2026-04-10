using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Elements.Guilds
{
    public class DefaultGuild : Guild
    {
        public override DefaultGuild Clone()
        {
            return new DefaultGuild();
        }

        public override Task Apply(IGameContext gameContext, int playerId)
        {
            return Task.CompletedTask;
        }

        public override Task Unapply(IGameContext gameContext, int playerId)
        {
            return Task.CompletedTask;
        }
    }
}
