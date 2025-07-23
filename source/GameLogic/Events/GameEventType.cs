using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Events
{
    public enum GameEventType
    {
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

        BuildingCostCalculated,
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
