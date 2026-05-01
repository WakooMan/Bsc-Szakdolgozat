using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Common;
using System;
using System.Numerics;

namespace SevenWonders.Game.Logic.PlayerActions
{
    public class ChooseWonderAction : IPlayerAction
    {
        public string Name => m_wonder.Name;
        public int Id => 9;
        public Wonder Wonder => m_wonder;
        public Player Player => m_player();

        public ChooseWonderAction() { }
        public ChooseWonderAction(Wonder wonder, List<Wonder> wonders, Func<Player> player)
        {
            ArgumentChecker.CheckNull(wonder, nameof(wonder));
            ArgumentChecker.CheckNull(wonders, nameof(wonders));
            ArgumentChecker.CheckNull(player, nameof(player));

            m_wonder = wonder;
            m_wonders = wonders;
            m_player = player;
        }

        public bool CanPerform(IGameContext gameContext)
        {
            return m_wonders.Contains(m_wonder);
        }

        public bool DoPlayerAction(IGameContext gameContext)
        {
            ArgumentChecker.CheckPredicateForOperation(() => !m_wonders.Contains(m_wonder), "Wonder list does not contain the wonder! Action cannot be performed!");

            m_player().Wonders.Add(m_wonder);
            m_wonders.Remove(m_wonder);
            gameContext.EventManager.Publish(new OnWonderChosen(m_player(), m_wonder));
            return true;
        }

        private readonly Wonder m_wonder;
        private readonly Func<Player> m_player;
        private readonly List<Wonder> m_wonders;
    }
}
