using GameLogic;
using GameLogic.Ages;
using GameLogic.Elements.Military;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public class MediumGlobalInfoEncoder: IMediumGlobalInfoEncoder
    {
        public MediumGlobalInfoEncoder(IGame game)
        {
            m_game = game;
        }

        public void EncodeGlobalInfo(List<float> vector, PhaseIndicator phaseIndicator)
        {
            float[] phaseStates = m_phaseStates[phaseIndicator];
            for (int i = 0; i < phaseStates.Length; i++)
            {
                vector.Add(phaseStates[i]);
            }

            AgesEnum agesEnum = m_game.Context.AgeHandler.CurrentAge.Age;
            float[] ageStates = m_ageStates[agesEnum];
            for (int i = 0; i < ageStates.Length; i++)
            {
                vector.Add(ageStates[i]);
            }

            var militaryBoard = m_game.Context.MilitaryBoard;
            if (militaryBoard is not null && militaryBoard.Fields.Count > 0)
            {
                int shieldIndex = militaryBoard.Fields.IndexOf(MilitaryField.Shield);
                int middle = militaryBoard.Fields.Count / 2;
                vector.Add((shieldIndex - middle) / (float)middle);
            }
            else
            {
                vector.Add(0f);
            }

            vector.Add(m_game.Context.AgeHandler.CurrentAge.Composition.AllCards.Count / 20f);
        }

        private readonly OrderedDictionary<AgesEnum, float[]> m_ageStates = new OrderedDictionary<AgesEnum, float[]>
        {
            { AgesEnum.I, [1,0,0]},
            { AgesEnum.II, [0,1,0]},
            { AgesEnum.III, [0,0,1]},
        };
        private readonly OrderedDictionary<PhaseIndicator, float[]> m_phaseStates = new OrderedDictionary<PhaseIndicator, float[]>
        {
            { PhaseIndicator.ChooseCard, [1,0]},
            { PhaseIndicator.ChooseAction, [0,1]}
        };
        private readonly IGame m_game;
    }
}
