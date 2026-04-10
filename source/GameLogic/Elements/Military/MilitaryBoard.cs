using GameLogic.Elements.Modifiers;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GameLogic.Elements.Military
{
    public class MilitaryBoard : IMilitaryBoard
    {
        public List<MilitaryField> Fields { get; set; }
        public List<MilitaryCard> MilitaryCards { get; set; }
        public List<Development> Developments { get; set; }
        public MilitaryBoard()
        {
            Fields = new List<MilitaryField>();
            MilitaryCards = new List<MilitaryCard>();
            Developments = new List<Development>();
            m_keyValuePairs = new Dictionary<Player, PlayerSide>();
            m_currentMilitaryCard = null;
        }

        public void Initialize(ICollection<Player> players, ICollection<Development> developments, IGameContext gameContext)
        {
            m_keyValuePairs.Clear();
            m_keyValuePairs.Add(players.First(), PlayerSide.First);
            m_keyValuePairs.Add(players.Last(), PlayerSide.Second);
            Developments.AddRange(developments);
            gameContext.EventManager.Subscribe<OnScientificProgress>((args) => OnScientificProgress(gameContext, args).GetAwaiter().GetResult());
            gameContext.EventManager.Subscribe<OnMilitaryAdvanced>((args) => OnMilitaryAdvanced(gameContext.EventManager, args).GetAwaiter().GetResult());
            gameContext.EventManager.Subscribe<OnMilitaryTokenReachedThreshold>((eventArgs) => OnMilitaryTokenReachedThreshold(gameContext, eventArgs));
        }

        private async Task OnMilitaryAdvanced(IEventManager eventManager, OnMilitaryAdvanced eventArgs)
        {
            int index = Fields.IndexOf(MilitaryField.Shield);
            PlayerSide playerSide = m_keyValuePairs[eventArgs.Player];
            int newIdx = Math.Clamp(index + ((int)playerSide * eventArgs.Advancement), 0, Fields.Count - 1);
            Fields[newIdx] = MilitaryField.Shield;
            Fields[index] = MilitaryField.None;

            List<MilitaryCard> militaryCards = new List<MilitaryCard>();

            int minIdx = Math.Min(index, newIdx);
            int maxIdx = Math.Max(index, newIdx);

            for (int i = minIdx; i <= maxIdx; i++)
            {
                MilitaryCard? militaryCard = MilitaryCards.FirstOrDefault(militaryCard => militaryCard.IndexStart <= i && militaryCard.IndexEnd >= i);
                if (militaryCard is not null && !militaryCards.Contains(militaryCard))
                {
                    militaryCards.Add(militaryCard);
                }
            }

            MilitaryCard? previousMilitaryCard = m_currentMilitaryCard;
            m_currentMilitaryCard = militaryCards.FirstOrDefault(militaryCard => militaryCard.IndexStart <= newIdx && militaryCard.IndexEnd >= newIdx);
            await eventManager.PublishAsync(new OnMilitaryBoardChanged(Fields));
            await eventManager.PublishAsync(new OnMilitaryTokenReachedThreshold(eventArgs.Player, militaryCards, previousMilitaryCard, m_currentMilitaryCard));

            if (newIdx == 0 || newIdx == Fields.Count - 1)
            {
                await eventManager.PublishAsync(new MilitaryVictory(eventArgs.Player.Name));
            }
        }

        private void OnMilitaryTokenReachedThreshold(IGameContext gameContext, OnMilitaryTokenReachedThreshold eventArgs)
        {
            eventArgs.MilitaryCards.ForEach(militaryCard =>
            {
                if (militaryCard != eventArgs.CurrentMilitaryCard)
                {
                    if (militaryCard != eventArgs.PreviousMilitaryCard)
                    {
                        militaryCard.Apply(gameContext);
                    }
                    militaryCard.Unapply(gameContext);
                }
            });

            if (eventArgs.CurrentMilitaryCard is not null)
            {
                eventArgs.CurrentMilitaryCard.Apply(gameContext);
            }
        }

        private async Task OnScientificProgress(IGameContext gameContext, OnScientificProgress eventArgs)
        {
            var disciplines = eventArgs.Disciplines;
            Player player = gameContext.TurnHandler.GetPlayer(eventArgs.PlayerId);
            if (disciplines.ContainsKey(eventArgs.Discipline.GetType()) && disciplines[eventArgs.Discipline.GetType()] == 2)
            {
                await gameContext.EventManager.PublishAsync(new OnChooseObjects("Válassz fejlesztést", Developments.Select(dev => dev.Name).ToArray(), true));
                await gameContext.PlayerActionHandler.HandlePlayerActions(gameContext, player, Developments.Select(dev => {
                    IPlayerAction action = new ChooseDevelopmentAction(player, dev, Developments);
                    return action;
                }).ToArray());
            }

            if (disciplines.Count >= 6)
            {
                await gameContext.EventManager.PublishAsync(new ScientificVictory(player.Name));
            }
        }

        [XmlIgnore]
        private readonly Dictionary<Player, PlayerSide> m_keyValuePairs;

        private MilitaryCard? m_currentMilitaryCard;
    }
}
