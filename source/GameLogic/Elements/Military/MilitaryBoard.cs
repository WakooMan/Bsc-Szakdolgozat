using GameLogic.Elements.Modifiers;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
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

        public void Initialize(ICollection<Player> players, ICollection<Development> developments, IGameContext gameContext)
        {
            m_keyValuePairs.Clear();
            m_keyValuePairs.Add(players.First(), PlayerSide.First);
            m_keyValuePairs.Add(players.Last(), PlayerSide.Second);
            Developments.AddRange(developments);
            gameContext.EventManager.Subscribe<OnScientificProgress>((args) => OnScientificProgress(gameContext, args));
            gameContext.EventManager.Subscribe<OnMilitaryTokenReachedThreshold>(OnMilitaryTokenReachedThreshold);
            gameContext.EventManager.Subscribe<OnMilitaryAdvanced>((args) => OnMilitaryAdvanced(gameContext.EventManager, args));
        }

        private void OnMilitaryTokenReachedThreshold(OnMilitaryTokenReachedThreshold eventArgs)
        {
            eventArgs.MilitaryCards.ForEach(card => MilitaryCards.Remove(card));
        }

        private void OnMilitaryAdvanced(IEventManager eventManager, OnMilitaryAdvanced eventArgs)
        {
            int index = Fields.IndexOf(MilitaryField.Shield);
            PlayerSide playerSide = m_keyValuePairs[eventArgs.Player];
            int newIdx = Math.Clamp(index + ((int)playerSide * eventArgs.Advancement), 0, Fields.Count - 1);
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

            if (militaryCards.Count > 0)
            {
                eventManager.Publish(new OnMilitaryTokenReachedThreshold(militaryCards));
            }

            if (newIdx == 0 || newIdx == Fields.Count - 1)
            {
                eventManager.Publish(new MilitaryVictory());
            }
        }

        private void OnScientificProgress(IGameContext gameContext, OnScientificProgress eventArgs)
        {
            var disciplines = eventArgs.Player.Disciplines;
            if (disciplines.ContainsKey(eventArgs.Discipline.GetType()) && disciplines[eventArgs.Discipline.GetType()] == 2)
            {
                IPlayerAction playerAction = gameContext.PlayerActionReceiver.ReceivePlayerAction(eventArgs.Player, Developments.Select(dev => new ChooseDevelopmentAction(eventArgs.Player, dev)).ToArray());
                if (playerAction.CanPerform(gameContext))
                {
                    playerAction.DoPlayerAction(gameContext);
                }
            }

            if (disciplines.Count >= 6)
            {
                gameContext.EventManager.Publish(new ScientificVictory());
            }
        }

        private readonly Dictionary<Player, PlayerSide> m_keyValuePairs;
    }
}
