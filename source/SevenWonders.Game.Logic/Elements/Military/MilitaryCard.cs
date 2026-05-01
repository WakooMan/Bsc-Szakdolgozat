using SevenWonders.Game.Logic.Elements.Effects;

namespace SevenWonders.Game.Logic.Elements.Military
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

        public void Apply(IGameContext gameContext)
        {
            Player owner = gameContext.TurnHandler.GetPlayer(OwnerId);
            Player opponent = gameContext.TurnHandler.GetPlayer(OpponentId);
            if (FirstApply)
            {
                EnemyLoseMoney.Apply(gameContext, opponent, owner);
            }

            opponent.MilitaryCards.Add(this);
        }

        public void Unapply(IGameContext gameContext)
        {
            Player owner = gameContext.TurnHandler.GetPlayer(OwnerId);
            Player opponent = gameContext.TurnHandler.GetPlayer(OpponentId);

            opponent.MilitaryCards.Remove(this);
        }

        public void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            VictoryPoints.OnCalculatePlayerProperties(playerProperties);
        }
    }
}
