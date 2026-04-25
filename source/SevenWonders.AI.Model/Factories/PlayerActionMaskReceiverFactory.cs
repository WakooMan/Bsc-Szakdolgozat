using GameLogic;
using SevenWonders.AI.Model.Services;

namespace SevenWonders.AI.Model.Factories
{
    public class PlayerActionMaskReceiverFactory : IPlayerActionMaskReceiverFactory
    {
        public PlayerActionMaskReceiverFactory(IGame game)
        {
            m_game = game;
        }

        public IPlayerActionMaskReceiver Create()
        {
            return new PlayerActionMaskReceiver(m_game);
        }

        private readonly IGame m_game;
    }
}
