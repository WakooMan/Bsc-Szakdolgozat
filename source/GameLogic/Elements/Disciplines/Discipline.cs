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
            await gameContext.EventManager.PublishAsync(new OnScientificProgress(player.Id, (await player.GetPlayerProperties()).Disciplines, this, player.PlayerActionReceiver));
        }

        public async Task Unapply(IGameContext gameContext, int playerId)
        {
            Player player = gameContext.TurnHandler.GetPlayer(playerId);
            await gameContext.EventManager.PublishAsync(new OnScientificRegress(player.Id, (await player.GetPlayerProperties()).Disciplines));
        }

        public Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            playerProperties.AddDiscipline(this);
            return Task.CompletedTask;
        }
    }
}
