using GameLogic.Events.GameEvents;
using System.Xml.Serialization;

namespace GameLogic.Elements.Disciplines
{
    [XmlInclude(typeof(Building)),
     XmlInclude(typeof(Geography)),
     XmlInclude(typeof(Healing)),
     XmlInclude(typeof(Mechanics)),
     XmlInclude(typeof(Physics)),
     XmlInclude(typeof(Trading)),
     XmlInclude(typeof(Writing)),]
    public abstract class Discipline
    {
        public abstract Discipline Clone();

        public async Task Apply(IGameContext gameContext, int playerId)
        {
            Player player = gameContext.TurnHandler.GetPlayer(playerId);
            m_action = (properties) => OnCalculatePlayerProperties(player, properties);
            gameContext.EventManager.Subscribe(m_action);
            await gameContext.EventManager.PublishAsync(new OnScientificProgress(player.Id, (await player.GetPlayerProperties()).Disciplines, this, gameContext.PlayerActionReceiver));
        }

        public async Task Unapply(IGameContext gameContext, int playerId)
        {
            Player player = gameContext.TurnHandler.GetPlayer(playerId);
            await gameContext.EventManager.PublishAsync(new OnScientificRegress(player.Id, (await player.GetPlayerProperties()).Disciplines));

            if (m_action is not null)
            {
                gameContext.EventManager.Unsubscribe(m_action);
            }
        }

        private void OnCalculatePlayerProperties(Player player, OnCalculatePlayerProperties properties)
        {
            if (properties.PlayerProperties.Player.Id == player.Id)
            {
                properties.PlayerProperties.AddDiscipline(this);
            }
        }

        private Action<OnCalculatePlayerProperties>? m_action;
    }
}
