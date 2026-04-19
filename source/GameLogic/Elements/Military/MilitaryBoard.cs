using GameLogic.Elements.Modifiers;
using GameLogic.Events.GameEvents;

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
            m_currentMilitaryCard = null;
        }

        public void Initialize(ICollection<Player> players, ICollection<Development> developments)
        {
            Developments.AddRange(developments);
        }

        public void OnUpdate(IGameContext gameContext, PlayerProperties player1, PlayerProperties player2)
        {
            OnMilitaryAdvanced(gameContext, player1, player2);
        }

        private void OnMilitaryAdvanced(IGameContext gameContext, PlayerProperties player1, PlayerProperties player2)
        {
            int index = Fields.IndexOf(MilitaryField.Shield);
            int diff = player1.Strength - player2.Strength;
            int middle = Fields.Count / 2;
            int newIdx = Math.Clamp(middle + diff, 0, Fields.Count - 1);
            if (newIdx != index)
            {
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
                gameContext.EventManager.Publish(new OnMilitaryBoardChanged(Fields));
                OnMilitaryTokenReachedThreshold evt = new OnMilitaryTokenReachedThreshold(militaryCards, previousMilitaryCard, m_currentMilitaryCard);
                OnMilitaryTokenReachedThreshold(gameContext, evt);
                gameContext.EventManager.Publish(evt);

                if (newIdx == 0 || newIdx == Fields.Count - 1)
                {
                    gameContext.EventManager.Publish(new MilitaryVictory(gameContext.TurnHandler.CurrentPlayer.GetPlayerProperties(gameContext.TurnHandler.OpponentPlayer)));
                }
            }
        }

        private void OnMilitaryTokenReachedThreshold(IGameContext gameContext, OnMilitaryTokenReachedThreshold eventArgs)
        {
            foreach(MilitaryCard militaryCard in eventArgs.MilitaryCards)
            {
                if (militaryCard != eventArgs.CurrentMilitaryCard)
                {
                    if (militaryCard != eventArgs.PreviousMilitaryCard)
                    {
                        militaryCard.Apply(gameContext);
                    }
                    militaryCard.Unapply(gameContext);
                }
            }

            if (eventArgs.CurrentMilitaryCard is not null)
            {
                eventArgs.CurrentMilitaryCard.Apply(gameContext);
            }
        }

        private MilitaryCard? m_currentMilitaryCard;
    }
}
