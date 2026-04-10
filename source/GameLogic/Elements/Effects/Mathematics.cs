using GameLogic.Events.GameEvents;

namespace GameLogic.Elements.Effects
{
    public class Mathematics : Effect
    {
        public VictoryPoints VictoryPointsPerDevelopment { get; set; }

        public Mathematics()
        {
            VictoryPointsPerDevelopment = new VictoryPoints();
        }

        private Mathematics(Mathematics mathematics)
        {
            VictoryPointsPerDevelopment = mathematics.VictoryPointsPerDevelopment.Clone();
        }

        public override Mathematics Clone()
        {
            return new Mathematics(this);
        }

        public override Task Apply(IGameContext gameContext, int playerId)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.EventManager.Subscribe<OnGameEnded>((args) => OnGameEnded(player, args));
            return Task.CompletedTask;
        }

        private void OnGameEnded(Player player, OnGameEnded args)
        {
            args.Points[player] += VictoryPointsPerDevelopment.Points * player.Developments.Count;
        }
    }
}
