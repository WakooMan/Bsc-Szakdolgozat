using SevenWonders.Web.Server.Contract.DataTransferObjects;

namespace SevenWondersUI.Services.ConnectionStates
{
    public interface IConnectionContext
    {
        IConnectionState LoginState { get; }
        IConnectionState ConnectedState { get; }
        IConnectionState ConnectToSignalRState { get; }
        IConnectionState NotConnectedState { get; }
        IConnectionState ReceiveLobbiesState { get; }
        string AuthToken { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        LobbyDto[]? Lobbies { get; set; }
    }
}
