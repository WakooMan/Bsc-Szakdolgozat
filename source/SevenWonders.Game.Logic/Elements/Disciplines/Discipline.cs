using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.PlayerActions;
using System.Xml.Serialization;

namespace SevenWonders.Game.Logic.Elements.Disciplines
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

        public void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            playerProperties.AddDiscipline(this);
        }

        public void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            PlayerProperties playerProperties = owner.GetPlayerProperties(opponent);
            var disciplines = playerProperties.Disciplines;
            var developments = gameContext.MilitaryBoard.Developments;
            if (disciplines.ContainsKey(GetType()) && disciplines[GetType()] == 2)
            {
                if (developments.Count > 0)
                {
                    gameContext.EventManager.Publish(new OnChooseObjects("Válassz fejlesztést", developments.Select(dev => dev.Name).ToArray(), true));
                    gameContext.PlayerActionHandler.HandlePlayerActions(gameContext, owner, developments.Select(dev =>
                    {
                        IPlayerAction action = new ChooseDevelopmentAction(owner, opponent, dev, developments);
                        return action;
                    }).ToArray());
                }
            }

            if (disciplines.Count >= 6)
            {
                gameContext.EventManager.Publish(new ScientificVictory(playerProperties));
            }
        }
    }
}
