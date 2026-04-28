using CommunityToolkit.Maui.Views;
using SevenWonders.Common;
using SevenWonders.WebClient.Model;
using SevenWonders.WebClient.Model.Services;
using SevenWondersUI.Services;
using SevenWondersUI.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WebServer.Contract.DataTransferObjects;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWondersUI.ViewModels
{
    [QueryProperty(nameof(LobbyDto), "LobbyDto")]
    public class LobbyPageViewModel : BaseViewModel, IMessageHandler
    {
        private string m_lobbyName = string.Empty;
        private string m_chatInput = string.Empty;

        public string MembersTitle => "Tagok";
        public string ChatTitle => "Chat";
        public string SendButtonText => "Küldés";
        public string StartGameButtonText => "Játék indítása";
        public string LeaveButtonText => "Kilépés";

        public string LobbyName
        {
            get => m_lobbyName;
            set { m_lobbyName = value; OnPropertyChanged(); }
        }

        public string ChatInput
        {
            get => m_chatInput;
            set
            {
                m_chatInput = value;
                OnPropertyChanged();
                ((Command)SendChatCommand).ChangeCanExecute();
            }
        }

        public bool IsHost
        {
            get
            {
                return m_isHost;
            }
            set
            {
                if (m_isHost != value)
                {
                    m_isHost = value;
                    OnPropertyChanged();
                    ((Command)StartGameCommand).ChangeCanExecute();
                }
            }
        }

        public ObservableCollection<LobbyMemberModel> Members { get; } = new();
        public ObservableCollection<ChatMessageModel> ChatMessages { get; } = new();

        public LobbyDto LobbyDto
        {
            set { ApplyLobbyDto(value); }
        }

        public ICommand SendChatCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand LeaveCommand { get; }

        public LobbyPageViewModel(IClientHubService clientHubService, INavigationService navigationService)
        {
            m_clientHubService = clientHubService;
            m_navigationService = navigationService;

            m_lobbyStateUpdateMessageHandler = new LobbyResponseMessageHandlerDelegate<LobbyStateUpdateMessage>(OnLobbyStateUpdateMessageReceived);
            m_startGameResponseMessageHandler = new LobbyResponseMessageHandlerDelegate<StartGameResponseMessage>(OnStartGameResponseMessageReceived);
            m_leaveLobbyResponseMessageHandler = new LobbyResponseMessageHandlerDelegate<LeaveLobbyResponseMessage>(OnLeaveLobbyResponseMessageReceived);
            m_failureResponseMessageHandler = new LobbyResponseMessageHandlerDelegate<FailureResponseMessage>(OnFailureResponseMessageReceived);

            SendChatCommand = new Command(SendChat, () => !string.IsNullOrWhiteSpace(m_chatInput));
            StartGameCommand = new Command(StartGame, () => m_isHost && Members.Count == 2);
            LeaveCommand = new Command(LeaveLobby);
        }

        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_lobbyStateUpdateMessageHandler);
            registerer.Register(m_startGameResponseMessageHandler);
            registerer.Register(m_leaveLobbyResponseMessageHandler);
            registerer.Register(m_failureResponseMessageHandler);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_lobbyStateUpdateMessageHandler);
            registerer.Unregister(m_startGameResponseMessageHandler);
            registerer.Unregister(m_leaveLobbyResponseMessageHandler);
            registerer.Unregister(m_failureResponseMessageHandler);
        }

        private void ApplyLobbyDto(LobbyDto dto)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                LobbyName = dto.Name;

                Members.Clear();
                IsHost = false;
                foreach (LobbyPlayerDto member in dto.Members)
                {
                    LobbyMemberModel memberModel =new LobbyMemberModel
                    { 
                        UserName = member.UserName, 
                        IsHost = member.IsHost, 
                        IsLocalPlayer = member.UserName == m_clientHubService.UserName
                    };
                    Members.Add(memberModel);
                    if (memberModel.IsHost && memberModel.IsLocalPlayer)
                    {
                        IsHost = true;
                    }
                }
                ((Command)StartGameCommand).ChangeCanExecute();

                ChatMessages.Clear();
                foreach (ChatMessage msg in dto.ChatMessages)
                {
                    ChatMessages.Add(new ChatMessageModel(msg.UserName, msg.Message));
                }
            });
        }

        private async void SendChat()
        {
            string text = m_chatInput.Trim();
            if (string.IsNullOrEmpty(text))
                return;

            ChatInput = string.Empty;
            await m_clientHubService.InvokeLobbyCommand(new SendChatRequestMessage(text));
        }

        private async void StartGame()
        {
            await m_clientHubService.InvokeLobbyCommand(new StartGameRequestMessage());
        }

        private async void LeaveLobby()
        {
            await m_clientHubService.InvokeLobbyCommand(new LeaveLobbyRequestMessage());
        }

        private async Task<bool> OnLobbyStateUpdateMessageReceived(LobbyStateUpdateMessage message)
        {
            if (message.Success)
            {
                ApplyLobbyDto(message.LobbyDto);
            }
            return await Task.FromResult(message.Success);
        }

        private async Task<bool> OnStartGameResponseMessageReceived(StartGameResponseMessage message)
        {
            if (message.Success)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await m_navigationService.NavigateToAsync("//MultiplayerGamePage", new Dictionary<string, object>
                    {
                        { "Player1", message.Player1Name },
                        { "Player2", message.Player2Name },
                        { "Player1Type", message.Player1Type },
                        { "Player2Type", message.Player2Type },
                        { "StartingPlayerId", message.StartingPlayerId },
                        { "Seed", message.Seed }
                    });
                });
            }
            return message.Success;
        }

        private async Task<bool> OnLeaveLobbyResponseMessageReceived(LeaveLobbyResponseMessage message)
        {
            if (message.Success)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await m_navigationService.NavigateToAsync("//LobbyMainPage", new Dictionary<string, object>() { { "Lobbies", message.Lobbies } });
                });
            }
            return message.Success;
        }

        private async Task<bool> OnFailureResponseMessageReceived(FailureResponseMessage message)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var popup = new ErrorPopupWindow(new ErrorPopupViewModel(message.Message));
                var page = Application.Current?.MainPage;
                if (page is not null)
                {
                    await page.ShowPopupAsync(popup);
                }
            });
            return false;
        }

        private readonly LobbyResponseMessageHandlerDelegate<LobbyStateUpdateMessage> m_lobbyStateUpdateMessageHandler;
        private readonly LobbyResponseMessageHandlerDelegate<StartGameResponseMessage> m_startGameResponseMessageHandler;
        private readonly LobbyResponseMessageHandlerDelegate<LeaveLobbyResponseMessage> m_leaveLobbyResponseMessageHandler;
        private readonly LobbyResponseMessageHandlerDelegate<FailureResponseMessage> m_failureResponseMessageHandler;
        private readonly IClientHubService m_clientHubService;
        private readonly INavigationService m_navigationService;
        private bool m_isHost;
    }
}

