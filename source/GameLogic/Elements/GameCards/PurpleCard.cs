using GameLogic.Elements.Guilds;

namespace GameLogic.Elements.GameCards
{
    public class PurpleCard : Card
    {
        public Guild GuildObj { get; set; }
        public PurpleCard() : base()
        {
            GuildObj = new DefaultGuild();
        }
        private PurpleCard(PurpleCard purpleCard) : base(purpleCard)
        {
            GuildObj = purpleCard.GuildObj.Clone();
        }

        public override PurpleCard Clone()
        {
            return new PurpleCard(this);
        }
    }
}
