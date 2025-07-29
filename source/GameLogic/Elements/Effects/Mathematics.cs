using GameLogic.Events;

namespace GameLogic.Elements.Effects
{
    public class Mathematics : Effect
    {
        public VictoryPoints VictoryPointsPerDevelopment { get; set; }

        public Mathematics() { }

        private Mathematics(Mathematics mathematics)
        {
            VictoryPointsPerDevelopment = mathematics.VictoryPointsPerDevelopment.Clone();
        }

        public override Mathematics Clone()
        {
            return new Mathematics(this);
        }

        public override void Apply(IGameContext gameContext)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.EventManager.Subscribe(GameEventType.GameEnded, (args) => OnGameEnded(player, args));
        }

        private void OnGameEnded(Player player, EventArgs args)
        {
            if (args is OnGameEnded onGameEnded)
            {
                onGameEnded.VictoryPoints[player] += VictoryPointsPerDevelopment.Points * player.Developments.Count;
            }
        }
    }
}
