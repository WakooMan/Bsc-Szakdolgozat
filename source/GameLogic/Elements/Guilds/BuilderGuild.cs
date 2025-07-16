namespace GameLogic.Elements.Guilds
{
    public class BuilderGuild : Guild
    {
        public override Guild Clone()
        {
            return new BuilderGuild();
        }
    }
}
