using System.Text.Json.Serialization;

namespace SevenWonders.AI.Model.Messages
{
    public class GameResetResponse
    {
        [JsonPropertyName("state")]
        public List<float> State { get; set; } = [];

        [JsonPropertyName("mask")]
        public List<int> Mask { get; set; } = [];

        [JsonPropertyName("terminated")]
        public bool Terminated { get; set; }

        [JsonPropertyName("opponent_type")]
        public int OpponentType { get; set; }
    }
}
