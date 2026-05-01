using SevenWonders.Game.Logic.GameStructures;
using SevenWonders.Common;
using System.Xml.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace SevenWonders.Game.Logic.Handlers
{
    public class CardCompositionFileHandler : ICardCompositionFileHandler
    {
        private readonly string m_compositionResourcePath;
        public CardCompositionFileHandler(string compositionResourcePath)
        {
            ArgumentChecker.CheckNullOrEmpty(compositionResourcePath, nameof(compositionResourcePath));

            m_compositionResourcePath = compositionResourcePath;
        }

        [ExcludeFromCodeCoverage]
        public void SetCompositionForCards(List<ICardNode> cardNodes)
        {
            try
            {
                ArgumentChecker.CheckNull(cardNodes, nameof(cardNodes));
                var assembly = typeof(CardCompositionFileHandler).Assembly;

                using (Stream? stream = assembly.GetManifestResourceStream(m_compositionResourcePath))
                {
                    if (stream is not null)
                    {
                        using var reader = new StreamReader(stream);
                        string[] lines = reader.ReadToEnd()
                                               .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        ArgumentChecker.CheckPredicateForOperation(() => cardNodes.Count != lines.Length, $"File line number is not equal to card number! File number: {lines.Length}, Card number: {cardNodes.Count}");

                        for (int i = 0; i < lines.Length; i++)
                        {
                            string[] splitted = lines[i].Split(";");
                            if (splitted.Length != 3)
                            {
                                throw new InvalidOperationException($"All the lines should contain exactly one semicolon in the file: {m_compositionResourcePath}");
                            }
                            bool hidden = bool.Parse(splitted[0]);
                            List<int> coveredBy = splitted[1].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();
                            string nodeName = splitted[2];
                            cardNodes[i].Hidden = hidden;
                            cardNodes[i].NodeName = nodeName;
                            foreach (int n in coveredBy)
                            {
                                cardNodes[i].AddParent(cardNodes[n]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
