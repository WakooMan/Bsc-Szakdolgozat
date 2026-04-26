namespace SevenWonders.AI.Model.Services.Encoders.Structs
{
    public readonly struct ScienceAnalysis
    {
        public int CompleteSets { get; init; }
        public int MaxSingle { get; init; }
        public int Distinct { get; init; }
        public int Total { get; init; }
        public int DisciplineCount { get; init; }
    }
}
