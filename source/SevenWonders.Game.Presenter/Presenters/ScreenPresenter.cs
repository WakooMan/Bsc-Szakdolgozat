using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Presenter.Connectors;
using SkiaSharp.Views.Maui;
using SevenWonders.Game.Engine.SceneObjects;
using SevenWonders.Game.Engine.InputHandling;
using SevenWonders.Game.Engine.SceneHandling;

namespace SevenWonders.Game.Presenter.Presenters
{
    public class ScreenPresenter : IPresenter
    {
        public ScreenPresenter(IGameEngineReceiver gameEngineReceiver, IEventManager eventManager, IGameOverHandler gameOverHandler)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
            m_gameOverHandler = gameOverHandler;
        }

        public void Initialize()
        {
            m_loadingScreen = m_gameEngineReceiver.ReceiveGraphicsLayer("LoadingScreen");
            m_scienceGameOverScreen = m_gameEngineReceiver.ReceiveGraphicsLayer("ScienceGameOverScreen");
            m_sciencePlayerName = m_gameEngineReceiver.ReceiveTextLabel("ScienceName");
            m_militaryGameOverScreen = m_gameEngineReceiver.ReceiveGraphicsLayer("MilitaryGameOverScreen");
            m_militaryPlayerName = m_gameEngineReceiver.ReceiveTextLabel("MilitaryName");
            m_citizenGameOverScreen = m_gameEngineReceiver.ReceiveGraphicsLayer("CitizenGameOverScreen");
            m_citizenGameResult = m_gameEngineReceiver.ReceiveTextLabel("CitizenResult");
            m_citizenFirstPlayerName = m_gameEngineReceiver.ReceiveTextLabel("CitizenFirstPlayerName");
            m_citizenFirstPlayerVictoryPoints = m_gameEngineReceiver.ReceiveTextLabel("CitizenFirstPlayerVictoryPoints");
            m_citizenFirstPlayerBlueCardNumber = m_gameEngineReceiver.ReceiveTextLabel("CitizenFirstPlayerBlueCardNumber");
            m_citizenSecondPlayerName = m_gameEngineReceiver.ReceiveTextLabel("CitizenSecondPlayerName");
            m_citizenSecondPlayerVictoryPoints = m_gameEngineReceiver.ReceiveTextLabel("CitizenSecondPlayerVictoryPoints");
            m_citizenSecondPlayerBlueCardNumber = m_gameEngineReceiver.ReceiveTextLabel("CitizenSecondPlayerBlueCardNumber");
            m_citizenBackToMenu = m_gameEngineReceiver.ReceiveButton("CitizenBackToMenu");
            m_militaryBackToMenu = m_gameEngineReceiver.ReceiveButton("MilitaryBackToMenu");
            m_scienceBackToMenu = m_gameEngineReceiver.ReceiveButton("ScienceBackToMenu");
        }

        public void SubscribeToEvents()
        {
            m_eventManager.Subscribe<OnGameStarted>(eventObj =>
            {
                if (m_loadingScreen is not null)
                {
                    m_loadingScreen.Visible = false;
                }
            });

            m_eventManager.Subscribe<MilitaryVictory>(eventObj =>
            {
                if (m_militaryGameOverScreen is not null && m_militaryPlayerName is not null && m_militaryBackToMenu is not null)
                {
                    m_militaryBackToMenu.ClickedEvent += OnClickBackToMenu;
                    m_militaryPlayerName.TextProperties.Text = $"{eventObj.PlayerProperties.Owner.Name}";
                    m_militaryGameOverScreen.Visible = true;
                }
            });

            m_eventManager.Subscribe<ScientificVictory>(eventObj =>
            {
                if (m_scienceGameOverScreen is not null && m_sciencePlayerName is not null && m_scienceBackToMenu is not null)
                {
                    m_scienceBackToMenu.ClickedEvent += OnClickBackToMenu;
                    m_sciencePlayerName.TextProperties.Text = $"{eventObj.PlayerProperties.Owner.Name}";
                    m_scienceGameOverScreen.Visible = true;
                }
            });

            m_eventManager.Subscribe<OnGameEnded>(eventObj =>
            {
                if (m_citizenGameOverScreen is not null && 
                    m_citizenGameResult is not null &&
                    m_citizenFirstPlayerName is not null &&
                    m_citizenFirstPlayerVictoryPoints is not null &&
                    m_citizenFirstPlayerBlueCardNumber is not null &&
                    m_citizenSecondPlayerName is not null &&
                    m_citizenSecondPlayerVictoryPoints is not null &&
                    m_citizenSecondPlayerBlueCardNumber is not null &&
                    m_citizenBackToMenu is not null)
                {
                    m_citizenBackToMenu.ClickedEvent += OnClickBackToMenu;

                    m_citizenFirstPlayerName.TextProperties.Text = eventObj.FirstPlayer.Owner.Name;
                    m_citizenFirstPlayerVictoryPoints.TextProperties.Text = eventObj.FirstPlayer.VictoryPoints.ToString();
                    int numberOfBlueCardsPlayer1 = eventObj.FirstPlayer.Owner.Cards.OfType<BlueCard>().Count();
                    m_citizenFirstPlayerBlueCardNumber.TextProperties.Text = numberOfBlueCardsPlayer1.ToString();

                    m_citizenSecondPlayerName.TextProperties.Text = eventObj.SecondPlayer.Owner.Name;
                    m_citizenSecondPlayerVictoryPoints.TextProperties.Text = eventObj.SecondPlayer.VictoryPoints.ToString();
                    int numberOfBlueCardsPlayer2 = eventObj.SecondPlayer.Owner.Cards.OfType<BlueCard>().Count();
                    m_citizenSecondPlayerBlueCardNumber.TextProperties.Text = numberOfBlueCardsPlayer2.ToString();

                    string resultText = string.Empty;
                    if (eventObj.FirstPlayer.VictoryPoints > eventObj.SecondPlayer.VictoryPoints)
                    {
                        resultText = "Győzelem!";
                        m_citizenFirstPlayerName.TextProperties.Bold = true;
                    }
                    else if (eventObj.FirstPlayer.VictoryPoints < eventObj.SecondPlayer.VictoryPoints)
                    {
                        resultText = "Győzelem!";
                        m_citizenSecondPlayerName.TextProperties.Bold = true;
                    }
                    else
                    {
                        if (numberOfBlueCardsPlayer1 > numberOfBlueCardsPlayer2)
                        {
                            resultText = "Győzelem!";
                            m_citizenFirstPlayerName.TextProperties.Bold = true;
                        }
                        else if (numberOfBlueCardsPlayer1 < numberOfBlueCardsPlayer2)
                        {
                            resultText = "Győzelem!";
                            m_citizenSecondPlayerName.TextProperties.Bold = true;
                        }
                        else
                        {
                            resultText = "Döntetlen!";
                        }
                    }

                    m_citizenGameResult.TextProperties.Text = resultText;
                    m_citizenGameOverScreen.Visible = true;
                }
            });
        }

        private void OnClickBackToMenu(IInteractiveObject interactiveObject, SKTouchEventArgs eventArgs)
        {
            m_gameOverHandler.OnGameOver();
            interactiveObject.ClickedEvent -= OnClickBackToMenu;
        }

        private TextLabel? m_militaryPlayerName;
        private TextLabel? m_sciencePlayerName;
        private TextLabel? m_citizenGameResult;
        private TextLabel? m_citizenFirstPlayerName;
        private TextLabel? m_citizenFirstPlayerVictoryPoints;
        private TextLabel? m_citizenFirstPlayerBlueCardNumber;
        private TextLabel? m_citizenSecondPlayerName;
        private TextLabel? m_citizenSecondPlayerVictoryPoints;
        private TextLabel? m_citizenSecondPlayerBlueCardNumber;
        private ButtonObject? m_citizenBackToMenu;
        private ButtonObject? m_militaryBackToMenu;
        private ButtonObject? m_scienceBackToMenu;
        private GraphicsLayer? m_loadingScreen;
        private GraphicsLayer? m_scienceGameOverScreen;
        private GraphicsLayer? m_militaryGameOverScreen;
        private GraphicsLayer? m_citizenGameOverScreen;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEventManager m_eventManager;
        private readonly IGameOverHandler m_gameOverHandler;
    }
}
