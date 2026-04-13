namespace SevenWondersUI.ViewModels
{
    public class ChatMessageModel
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public string DisplayText => $"[{UserName}]: {Message}";

        public ChatMessageModel(string userName, string message)
        {
            UserName = userName;
            Message = message;
        }
    }
}
