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
        }

        public void Initialize(ICollection<Player> players, ICollection<Development> developments, IGameContext gameContext)
        {
            m_keyValuePairs.Clear();
            m_keyValuePairs.Add(players.First(), PlayerSide.First);
            m_keyValuePairs.Add(players.Last(), PlayerSide.Second);
            Developments.AddRange(developments);
            gameContext.EventManager.Subscribe<OnScientificProgress>((args) => OnScientificProgress(gameContext, args).GetAwaiter().GetResult());
            gameContext.EventManager.Subscribe<OnMilitaryTokenReachedThreshold>(OnMilitaryTokenReachedThreshold);
            gameContext.EventManager.Subscribe<OnMilitaryAdvanced>((args) => OnMilitaryAdvanced(gameContext.EventManager, args).GetAwaiter().GetResult());
        }

        private void OnMilitaryTokenReachedThreshold(OnMilitaryTokenReachedThreshold eventArgs)
        {
            eventArgs.MilitaryCards.ForEach(card => MilitaryCards.Remove(card));
        }

        private async Task OnMilitaryAdvanced(IEventManager eventManager, OnMilitaryAdvanced eventArgs)
        {
            int index = Fields.IndexOf(MilitaryField.Shield);
            PlayerSide playerSide = m_keyValuePairs[eventArgs.Player];
            int newIdx = Math.Clamp(index + ((int)playerSide * eventArgs.Advancement), 0, Fields.Count - 1);
            Fields[newIdx] = MilitaryField.Shield;
            Fields[index] = MilitaryField.None;

            List<MilitaryCard> militaryCards = new List<MilitaryCard>();
            int halfOfFieldCount = Fields.Count / 2;

            if (playerSide == PlayerSide.First && newIdx > halfOfFieldCount)
            {
                militaryCards = MilitaryCards.Where(militaryCard => militaryCard.IndexStart > halfOfFieldCount &&
                    ((militaryCard.IndexStart <= newIdx && militaryCard.IndexEnd >= newIdx) || (militaryCard.IndexEnd < newIdx))).ToList();
            }
            else if (playerSide == PlayerSide.Second && newIdx < halfOfFieldCount)
            {
                militaryCards = MilitaryCards.Where(militaryCard => militaryCard.IndexEnd < halfOfFieldCount &&
                    ((militaryCard.IndexStart <= newIdx && militaryCard.IndexEnd >= newIdx) || (militaryCard.IndexStart > newIdx))).ToList();
            }

            await eventManager.PublishAsync(new OnMilitaryBoardChanged(Fields));

            if (militaryCards.Count > 0)
            {
                await eventManager.PublishAsync(new OnMilitaryTokenReachedThreshold(militaryCards));
            }

            if (newIdx == 0 || newIdx == Fields.Count - 1)
            {
                await eventManager.PublishAsync(new MilitaryVictory(eventArgs.Player));
            }
        }

        private async Task OnScientificProgress(IGameContext gameContext, OnScientificProgress eventArgs)
        {
            var disciplines = eventArgs.Player.Disciplines;
            if (disciplines.ContainsKey(eventArgs.Discipline.GetType()) && disciplines[eventArgs.Discipline.GetType()] == 2)
            {
                await gameContext.EventManager.PublishAsync(new OnChooseObjects("Choose Development", Developments.Select(dev => dev.Name).ToArray()));
                await gameContext.PlayerActionHandler.HandlePlayerActions(gameContext, eventArgs.Player, Developments.Select(dev => {
                    IPlayerAction action = new ChooseDevelopmentAction(eventArgs.Player, dev, Developments);
                    return action;
                }).ToArray());
            }

            if (disciplines.Count >= 6)
            {
                await gameContext.EventManager.PublishAsync(new ScientificVictory(eventArgs.Player));
            }
        }

        [XmlIgnore]
        private readonly Dictionary<Player, PlayerSide> m_keyValuePairs;
    }
}
