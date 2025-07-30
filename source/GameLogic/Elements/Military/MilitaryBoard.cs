using GameLogic.Elements.Modifiers;
using GameLogic.Events;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

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

        public void Initialize(ICollection<Player> players, ICollection<Development> developments, IEventManager eventManager)
        {
            m_keyValuePairs.Clear();
            m_keyValuePairs.Add(players.First(), PlayerSide.First);
            m_keyValuePairs.Add(players.Last(), PlayerSide.Second);
            Developments.AddRange(developments);
            eventManager.Subscribe(GameEventType.ScientificProgress, (args) => OnScientificProgress(eventManager, args));
            eventManager.Subscribe(GameEventType.MilitaryTokenReachedThreshold, OnMilitaryTokenReachedThreshold);
            eventManager.Subscribe(GameEventType.MilitaryAdvanced, (args) => OnMilitaryAdvanced(eventManager, args));
        }

        private void OnMilitaryTokenReachedThreshold(EventArgs args)
        {
            if (args is OnMilitaryTokenReachedThreshold eventArgs)
            {
                eventArgs.MilitaryCards.ForEach(card => MilitaryCards.Remove(card));
            }
        }

        private void OnMilitaryAdvanced(IEventManager eventManager, EventArgs args)
        {
            if (args is OnMilitaryAdvanced onMilitaryAdvanced)
            {
                int index = Fields.IndexOf(MilitaryField.Shield);
                PlayerSide playerSide = m_keyValuePairs[onMilitaryAdvanced.Player];
                int newIdx = Math.Clamp(index + ((int)playerSide * onMilitaryAdvanced.Advancement), 0, Fields.Count - 1);
                Fields[newIdx] = MilitaryField.Shield;
                Fields[index] = MilitaryField.None;

                List<MilitaryCard> militaryCards = new List<MilitaryCard>();

                if (playerSide == PlayerSide.First && newIdx > 10)
                {
                    militaryCards = MilitaryCards.Where(militaryCard => militaryCard.IndexStart > 10 &&
                        ((militaryCard.IndexStart <= newIdx && militaryCard.IndexEnd >= newIdx) || (militaryCard.IndexEnd < newIdx))).ToList();
                }
                else if (playerSide == PlayerSide.Second && newIdx < 10)
                {
                    militaryCards = MilitaryCards.Where(militaryCard => militaryCard.IndexEnd < 10 &&
                        ((militaryCard.IndexStart <= newIdx && militaryCard.IndexEnd >= newIdx) || (militaryCard.IndexStart > newIdx))).ToList();
                }

                if (militaryCards.Any())
                {
                    eventManager.Publish(GameEventType.MilitaryTokenReachedThreshold, new OnMilitaryTokenReachedThreshold(militaryCards));
                }

                if (newIdx == 0 || newIdx == Fields.Count - 1)
                {
                    eventManager.Publish(GameEventType.MilitaryVictory, new EventArgs());
                }
            }
        }

        private void OnScientificProgress(IEventManager eventManager, EventArgs args)
        {
            if (args is OnScientificProgress eventArgs)
            {
                var disciplines = eventArgs.Player.Disciplines;
                if (disciplines.ContainsKey(eventArgs.Discipline.GetType()) && disciplines[eventArgs.Discipline.GetType()] == 2)
                {
                    eventArgs.Player.Developments.Add(eventArgs.PlayerActionReceiver.ReceivePlayerAction<ChooseDevelopmentAction>(eventArgs.Player, Developments.Select(dev => new ChooseDevelopmentAction(dev)).ToArray()).Development);
                }

                if (disciplines.Count >= 6)
                {
                    eventManager.Publish(GameEventType.ScientificVictory, new EventArgs());
                }

            }
        }

        private readonly Dictionary<Player, PlayerSide> m_keyValuePairs;
    }
}
