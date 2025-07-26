using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Events
{
    public enum GameEventType
    {
        BuildingCostCalculated,

        CardPicked,
        CardUnpicked,
        CardBuilt,
        CardSold,

        WonderBuilt,
        WonderEffectTriggered,
        WonderUsedToDestroyCard,

        MilitaryAdvanced,
        MilitaryTokenReachedThreshold,
        MilitaryVictory,

        ScienceSymbolAcquired,
        ScientificProgressChosen,
        ScientificVictory,

        CoinsGained,
        CoinsLost,
        ResourceDiscountApplied,

        MissingResourcesEvaluated,

        TurnStarted,
        TurnEnded,
        AgeEnded,
        GameEnded,
        VictoryChecked,

        GuildCardRevealed,
        CardDestroyed,
        ExtraTurnGranted,
        OpponentPlayedCardRemoved
    }
}
