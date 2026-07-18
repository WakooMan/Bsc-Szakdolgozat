using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Modifiers;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.PlayerActions
{
    public class ChooseDevelopmentAction : IPlayerAction
    {
        public string Name => m_development.Name;
        public bool ToDeck { get; }
        public int Id => 5;
        public ChooseDevelopmentAction() { }
        public ChooseDevelopmentAction(Player owner, Player opponent, Development development, List<Development> developments, bool toDeck)
        {
            ArgumentChecker.CheckNull(owner, nameof(owner));
            ArgumentChecker.CheckNull(opponent, nameof(opponent));
            ArgumentChecker.CheckNull(development, nameof(development));
            ArgumentChecker.CheckNull(developments, nameof(developments));

            m_owner = owner;
            m_opponent = opponent;
            m_development = development;
            m_developments = developments;
            ToDeck = toDeck;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return !m_owner.Developments.Contains(m_development) && m_developments.Contains(m_development);
        }

        public bool DoPlayerAction(IGameContext gameContext)
        {
            ArgumentChecker.CheckPredicateForOperation(() => m_owner.Developments.Contains(m_development), "Cannot perform action, because player already has the development!");
            ArgumentChecker.CheckPredicateForOperation(() => !m_developments.Contains(m_development), "Cannot perform action, because development list does not contain the development!");

            m_owner.Developments.Add(m_development);
            m_developments.Remove(m_development);
            gameContext.EventManager.Publish(new OnPlayerDevelopmentReceived(m_owner, m_development));
            gameContext.EventManager.Publish(new OnObjectChosen(m_developments.Select(dev => dev.Name).ToArray()));
            m_development.OnDevelopmentEstablished(gameContext, m_owner, m_opponent);
            return true;
        }

        private readonly List<Development> m_developments;
        private readonly Development m_development;
        private readonly Player m_owner;
        private readonly Player m_opponent;
    }
}
