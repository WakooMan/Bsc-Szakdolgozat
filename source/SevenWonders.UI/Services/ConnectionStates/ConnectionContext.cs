using SevenWonders.Web.Client.Model;
using SevenWonders.Web.Client.Model.Services;
using SevenWonders.Web.Server.Contract.DataTransferObjects;

namespace SevenWondersUI.Services.ConnectionStates
{
    public class ConnectionContext: IConnectionContext
    {
        public IConnectionState LoginState { get; }
        public IConnectionState ConnectedState { get; }
        public IConnectionState ConnectToSignalRState { get; }
        public IConnectionState NotConnectedState { get; }
        public IConnectionState ReceiveLobbiesState { get; }
        public string AuthToken { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public LobbyDto[]? Lobbies { get; set; }

        public ConnectionContext(IClientHubService clientHubService, IAuthService authService, IClientMessageDispatcher clientMessageDispatcher)
        {
            NotConnectedState = new NotConnectedState(this);
            LoginState = new LoginState(this, authService);
            ConnectToSignalRState = new ConnectToSignalRState(this, clientHubService);
            ReceiveLobbiesState = new ReceiveLobbiesState(this, clientHubService, clientMessageDispatcher);
            ConnectedState = new ConnectedState(this);
            AuthToken = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            Lobbies = null;
        }
    }
}
