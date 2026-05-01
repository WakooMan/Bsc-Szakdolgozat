using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.AI.Model.DataModel;

namespace SevenWonders.AI.Model.DecisionRouter.CommonDecisionHandlers
{
    public class ChooseDevelopmentHandler: IDecisionHandler
    {
        public ChooseDevelopmentHandler(IWeightConfiguration weightConfiguration)
        {
            m_weightConfiguration = weightConfiguration;
        }

        public PlayerActionWrapper HandleDecisions(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            return playerActions.Where(w => w.CanPerform).Select(wrapper =>
            {
                ObjectWeight? objectWeight = m_weightConfiguration.ObjectWeights.DevelopmentWeights.Find(developmentWeight => developmentWeight.Name == wrapper.PlayerAction.Name);
                return objectWeight is not null ? (wrapper, objectWeight.Weight) : (wrapper, 0);
            }).OrderByDescending(tuple => tuple.Item2).First().Item1;
        }

        private readonly IWeightConfiguration m_weightConfiguration;
    }
}
