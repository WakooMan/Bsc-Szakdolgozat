using System.Text.Json.Serialization;

namespace SevenWonders.AI.Model.Messages
{
    public class GameStateResponse
    {
        [JsonPropertyName("state")]
        public List<float> State { get; set; } = [];

        [JsonPropertyName("mask")]
        public List<int> Mask { get; set; } = [];

        [JsonPropertyName("reward")]
        public float Reward { get; set; }

        [JsonPropertyName("terminated")]
        public bool Terminated { get; set; }

        [JsonPropertyName("aipoints")]
        public int AIPoints { get; set; }

        [JsonPropertyName("enemypoints")]
        public int EnemyPoints { get; set; }

        [JsonPropertyName("victorytype")]
        public int VictoryType { get; set; }
    }
}
