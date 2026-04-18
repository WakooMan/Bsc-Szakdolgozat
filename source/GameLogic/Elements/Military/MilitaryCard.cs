using GameLogic.Elements.Effects;

namespace GameLogic.Elements.Military
{
    public class MilitaryCard
    {
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public int OpponentId { get; set; }
        public EnemyLoseMoney EnemyLoseMoney { get; set; }
        public VictoryPoints VictoryPoints { get; set; }
        public int IndexStart { get; set; }
        public int IndexEnd { get; set; }
        public bool FirstApply { get; set; }

        public MilitaryCard()
        {
            Name = string.Empty;
            EnemyLoseMoney = new EnemyLoseMoney();
            VictoryPoints = new VictoryPoints();
            FirstApply = true;
            OwnerId = 0;
            OpponentId = 0;
        }

        public async Task Apply(IGameContext gameContext)
        {
            Player owner = gameContext.TurnHandler.GetPlayer(OwnerId);
            Player opponent = gameContext.TurnHandler.GetPlayer(OpponentId);
            if (FirstApply)
            {
                await EnemyLoseMoney.Apply(gameContext, opponent, owner);
            }

            opponent.MilitaryCards.Add(this);
        }

        public Task Unapply(IGameContext gameContext)
        {
            Player owner = gameContext.TurnHandler.GetPlayer(OwnerId);
            Player opponent = gameContext.TurnHandler.GetPlayer(OpponentId);

            opponent.MilitaryCards.Remove(this);
            return Task.CompletedTask;
        }

        public async Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            await VictoryPoints.OnCalculatePlayerProperties(playerProperties);
        }
    }
}
