using GameLogic.Elements;
using GameLogic.Interfaces;
using SevenWonders.AI.Model.DataModel;

namespace SevenWonders.AI.Model.DecisionRouter.CommonDecisionHandlers
{
    public class ChooseWonderDecisionHandler: IDecisionHandler
    {
        public ChooseWonderDecisionHandler(IWeightConfiguration weightConfiguration)
        {
            m_weightConfiguration = weightConfiguration;
        }

        public PlayerActionWrapper HandleDecisions(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            return playerActions.Select(wrapper =>
            {
                ObjectWeight? objectWeight = m_weightConfiguration.ObjectWeights.WonderWeights.Find(wonderWeight => wonderWeight.Name == wrapper.PlayerAction.Name);
                return objectWeight is not null ? (wrapper, objectWeight.Weight) : (wrapper, 0);
            }).OrderByDescending(tuple => tuple.Item2).First().Item1;
        }

        private readonly IWeightConfiguration m_weightConfiguration;
    }
}
