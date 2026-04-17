using GameLogic.Events.GameEvents;
using GameLogic.PlayerActions;
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

        public Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            playerProperties.AddDiscipline(this);
            return Task.CompletedTask;
        }

        public async Task Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            PlayerProperties playerProperties = await owner.GetPlayerProperties();
            var disciplines = playerProperties.Disciplines;
            var developments = gameContext.MilitaryBoard.Developments;
            if (disciplines.ContainsKey(GetType()) && disciplines[GetType()] == 2)
            {
                await gameContext.EventManager.PublishAsync(new OnChooseObjects("Válassz fejlesztést", developments.Select(dev => dev.Name).ToArray(), true));
                await gameContext.PlayerActionHandler.HandlePlayerActions(gameContext, owner, developments.Select(dev => {
                    IPlayerAction action = new ChooseDevelopmentAction(owner, opponent, dev, developments);
                    return action;
                }).ToArray());
            }

            if (disciplines.Count >= 6)
            {
                await gameContext.EventManager.PublishAsync(new ScientificVictory(owner.Name));
            }
        }
    }
}
