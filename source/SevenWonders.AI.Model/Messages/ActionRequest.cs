using System.Text.Json.Serialization;

namespace SevenWonders.AI.Model.Messages
{
    public class ActionRequest
    {
        [JsonPropertyName("action")]
        public int Action { get; set; }
    }
}
