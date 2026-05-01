using SevenWonders.Game.Logic.Elements;
using SevenWonders.AI.Model.Services.Encoders;
using SevenWonders.Common;

namespace SevenWonders.AI.Model.Services
{
    public class GameStateVectorReceiver: IGameStateVectorReceiver
    {
        public GameStateVectorReceiver(ICardCompositionEncoder cardCompositionEncoder, 
                                       IPlayerEncoder playerEncoder, 
                                       IGlobalInfoEncoder globalInfoEncoder)
        {
            m_playerEncoder = playerEncoder;
            m_cardCompositionEncoder = cardCompositionEncoder;
            m_globalInfoEncoder = globalInfoEncoder;
        }

        public void Initialize()
        {
            GameLog.Info("Initializing card composition encoder...");
            m_cardCompositionEncoder.InitializeComposition();
            GameLog.Info("Initialized.");
        }

        public List<float> Receive(PlayerProperties player1, PlayerProperties player2, PhaseIndicator phaseIndicator)
        {
            GameLog.Info($"Receive: Phase={phaseIndicator}, Player1={player1.Owner.Name}, Player2={player2.Owner.Name}");
            var vector = new List<float>();
            m_globalInfoEncoder.EncodeGlobalInfo(vector, phaseIndicator);
            m_playerEncoder.EncodePlayer(vector, player1);
            m_playerEncoder.EncodePlayer(vector, player2);
            m_cardCompositionEncoder.EncodeNodes(vector, player1, player2);
            GameLog.Info($"State vector size: {vector.Count}");
            return vector;
        }

        private readonly ICardCompositionEncoder m_cardCompositionEncoder;
        private readonly IPlayerEncoder m_playerEncoder;
        private readonly IGlobalInfoEncoder m_globalInfoEncoder;
    }
}
