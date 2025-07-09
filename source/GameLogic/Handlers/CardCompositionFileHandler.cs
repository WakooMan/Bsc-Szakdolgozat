using GameLogic.GameStructures;

namespace GameLogic.Handlers
{
    public class CardCompositionFileHandler : ICardCompositionFileHandler
    {
        private readonly string m_compositionFileName;
        public CardCompositionFileHandler(string compositionFileName)
        {
            m_compositionFileName = compositionFileName;
        }

        public void SetCompositionForCards(List<CardNode> cardNodes)
        {
            try
            {
                string[] lines = File.ReadAllLines(m_compositionFileName);
                foreach (string line in lines)
                {
                    string[] splitted = line.Split(";");
                    if (splitted.Length != 3)
                    {
                        throw new InvalidOperationException($"All the lines should contain exactly one semicolon in the file: {m_compositionFileName}");
                    }
                    int node = int.Parse(splitted[0]);
                    bool hidden = bool.Parse(splitted[1]);
                    int[] coveredBy = splitted[2].Split(",").Select(s => int.Parse(s)).ToArray();
                    cardNodes[node].Hidden = hidden;
                    foreach (int n in coveredBy)
                    {
                        cardNodes[node].AddParent(cardNodes[n]);
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Log and throw
            }
        }
    }
}
