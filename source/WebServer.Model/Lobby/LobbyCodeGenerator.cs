using System.Collections.Concurrent;
using SevenWonders.Common;

namespace WebServer.Model.Lobby
{
    public class LobbyCodeGenerator : ILobbyCodeGenerator
    {
        public LobbyCodeGenerator(IRandomGenerator randomGenerator)
        {
            m_randomGenerator = randomGenerator;
            m_dummyValue = 0;
        }

        public string GenerateUniqueCode()
        {
            string newCode;

            do
            {
                newCode = m_randomGenerator.Next(0, 1000000).ToString("D6");
            }
            while (!m_generatedCodes.TryAdd(newCode, m_dummyValue));

            return newCode;
        }

        public bool RemoveUniqueCode(string code)
        {
            return m_generatedCodes.TryRemove(code, out _);
        }

        private readonly ConcurrentDictionary<string, byte> m_generatedCodes = new();
        private readonly IRandomGenerator m_randomGenerator;
        private readonly byte m_dummyValue;
    }
}
