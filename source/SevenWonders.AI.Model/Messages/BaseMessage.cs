namespace SevenWonders.AI.Model.Messages
{
    public class BaseMessage
    {
        public MessageType MessageType { get; set; }
        public string Payload { get; set; }

        public BaseMessage()
        {
            Payload = string.Empty;
        }
    }
}
