using GameLogic.GameStructures;
using SevenWonders.Common;

namespace GameLogic.Handlers
{
    public class CardCompositionFileHandler : ICardCompositionFileHandler
    {
        private readonly string m_compositionFileName;
        public CardCompositionFileHandler(string compositionFileName)
        {
            ArgumentChecker.CheckNullOrEmpty(compositionFileName, nameof(compositionFileName));

            m_compositionFileName = compositionFileName;
        }

        public void SetCompositionForCards(List<ICardNode> cardNodes)
        {
            try
            {
                ArgumentChecker.CheckNull(cardNodes, nameof(cardNodes));
                string[] lines = File.ReadAllLines(m_compositionFileName);
                ArgumentChecker.CheckPredicateForOperation(() => cardNodes.Count != lines.Length, $"File line number is not equal to card number! File number: {lines.Length}, Card number: {cardNodes.Count}");
                
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] splitted = lines[i].Split(";");
                    if (splitted.Length != 2)
                    {
                        throw new InvalidOperationException($"All the lines should contain exactly one semicolon in the file: {m_compositionFileName}");
                    }
                    bool hidden = bool.Parse(splitted[0]);
                    List<int> coveredBy = splitted[1].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();
                    cardNodes[i].Hidden = hidden;
                    foreach (int n in coveredBy)
                    {
                        cardNodes[i].AddParent(cardNodes[n]);
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Log and throw
                throw ex;
            }
        }
    }
}
