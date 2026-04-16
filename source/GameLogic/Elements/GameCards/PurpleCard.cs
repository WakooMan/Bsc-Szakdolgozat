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

        public override async Task OnBuilt(IGameContext gameContext, int playerId)
        {
            await GuildObj.Apply(gameContext, playerId);
        }

        public override async Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            await GuildObj.OnCalculatePlayerProperties(playerProperties);
        }

        public override async Task OnBeforeGameEnded(Player owner, Player opponent)
        {
            await GuildObj.OnBeforeGameEnded(owner, opponent);
        }


    }
}
