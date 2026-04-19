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

        public override void OnBuilt(IGameContext gameContext, Player owner, Player opponent)
        {
           GuildObj.Apply(gameContext, owner, opponent);
        }

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
           GuildObj.OnCalculatePlayerProperties(playerProperties);
        }
    }
}
