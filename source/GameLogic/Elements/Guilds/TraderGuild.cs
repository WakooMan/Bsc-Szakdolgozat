namespace GameLogic.Elements.Guilds
{
    public class TraderGuild : Guild
    {
        public override Guild Clone()
        {
            return new TraderGuild();
        }
    }
}
