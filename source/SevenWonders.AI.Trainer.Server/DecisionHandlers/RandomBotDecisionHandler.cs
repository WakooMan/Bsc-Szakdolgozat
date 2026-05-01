using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Common;

namespace SevenWonders.AI.Model.DecisionRouter.DecisionHandlers
{
    public class RandomBotDecisionHandler : IRandomBotDecisionHandler
    {
        public RandomBotDecisionHandler(IRandomGeneratorFactory randomGeneratorFactory)
        {
            m_randomGenerator = randomGeneratorFactory.Create(RandomGeneratorType.Undeterministic, 0);
        }

        public PlayerActionWrapper HandleDecisions(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            GameLog.Info($"HandleDecisions: Player={player.Name}, ActionCount={playerActions.Count}");
            var array = playerActions.ToArray();
            List<int> indexes = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].CanPerform && !m_exclusions.Contains(array[i].PlayerAction.Name))
                {
                    indexes.Add(i);
                }
            }

            int randomActionIndex = m_randomGenerator.Next(0, indexes.Count - 1);
            GameLog.Info($"Chose action index={indexes[randomActionIndex]} out of {indexes.Count} performable actions. Action={array[indexes[randomActionIndex]].PlayerAction.GetType().Name}");
            return array[indexes[randomActionIndex]];
        }

        private readonly IRandomGenerator m_randomGenerator;
        private readonly List<string> m_exclusions = new List<string>()
        {
            "UnpickCard",
            "BackToTurnDecisions"
        };
    }
}
