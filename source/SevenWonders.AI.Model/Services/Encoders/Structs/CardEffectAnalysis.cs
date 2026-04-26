namespace SevenWonders.AI.Model.Services.Encoders.Structs
{
    public readonly struct CardEffectAnalysis
    {
        public int DeltaVP { get; init; }
        public int DeltaStrength { get; init; }
        public int DeltaCoins { get; init; }
        public int DeltaResourceCount { get; init; }
        public float FutureCostReduction { get; init; }
        public float DenialValue { get; init; }
        public Type? ScienceDiscipline { get; init; }
    }
}
