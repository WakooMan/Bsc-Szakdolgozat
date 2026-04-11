using SevenWonders.Common;

namespace WebServer.Model.Lobby
{
    public class LobbyCodeGenerator : ILobbyCodeGenerator
    {
        public LobbyCodeGenerator(ILobbyManager lobbyManager, IRandomGenerator randomGenerator)
        {
            m_lobbyManager = lobbyManager;
            m_randomGenerator = randomGenerator;
        }

        public string GenerateUniqueCode()
        {
            string[] existingCodes = m_lobbyManager.GetLobbyCodes();
            string newCode;
            bool isUnique = false;

            do
            {
                newCode = m_randomGenerator.Next(0, 1000000).ToString("D6");

                if (!existingCodes.Contains(newCode))
                {
                    isUnique = true;
                }
            }
            while (!isUnique);

            return newCode;
        }

        private readonly ILobbyManager m_lobbyManager;
        private readonly IRandomGenerator m_randomGenerator;
    }
}
