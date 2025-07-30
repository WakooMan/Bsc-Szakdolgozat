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

        MilitaryAdvanced,
        MilitaryTokenReachedThreshold,
        MilitaryVictory,

        ScientificProgress,
        ScientificVictory,

        TurnStarted,
        TurnEnded,
        AgeEnded,
        GameEnded,

        CardDestroyed,
        ExtraTurnGranted,
    }
}
