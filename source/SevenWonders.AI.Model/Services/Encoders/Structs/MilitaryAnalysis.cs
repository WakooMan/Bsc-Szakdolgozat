namespace SevenWonders.AI.Model.Services.Encoders.Structs
{
    public readonly struct MilitaryAnalysis
    {
        public float ShieldPosition { get; init; }
        public float WinProximity { get; init; }
        public int StrengthDiff { get; init; }
        public int BoardMiddle { get; init; }
        public int BoardLength { get; init; }
    }
}
