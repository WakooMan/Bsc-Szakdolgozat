using GameLogic.Events.GameEvents;

namespace GameLogic.Elements.Effects
{
    public class VictoryPoints : Effect
    {
        public int Points { get; set; }

        public VictoryPoints()
        {
            Points = 0;
        }

        private VictoryPoints(VictoryPoints victoryPoints)
        {
            Points = victoryPoints.Points;
        }

        public override VictoryPoints Clone()
        {
            return new VictoryPoints(this);
        }

        public override Task Apply(IGameContext gameContext, int playerId)
        {
            Player player = gameContext.TurnHandler.GetPlayer(playerId);
            m_action = (properties) => OnCalculatePlayerProperties(player, properties);
            gameContext.EventManager.Subscribe(m_action);
            return Task.CompletedTask;
        }

        public override Task Unapply(IGameContext gameContext, int playerId)
        {
            if (m_action is not null)
            {
                gameContext.EventManager.Unsubscribe(m_action);
            }
            return Task.CompletedTask;
        }

        private void OnCalculatePlayerProperties(Player player, OnCalculatePlayerProperties properties)
        {
            if (properties.PlayerProperties.Player == player)
            {
                properties.PlayerProperties.VictoryPoints += Points;
            }
        }

        private Action<OnCalculatePlayerProperties>? m_action;
    }
}
