using GameLogic.Events;
using GameLogic.Events.GameEvents;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;

namespace SevenWonders.Presenter.Presenters
{
    public class ScreenPresenter : IPresenter
    {
        public ScreenPresenter(IGameEngineReceiver gameEngineReceiver, IEventManager eventManager)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
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
                if (m_militaryGameOverScreen is not null && m_militaryPlayerName is not null)
                {
                    m_militaryPlayerName.Text = $"{eventObj.PlayerName}";
                    m_militaryGameOverScreen.Visible = true;
                }
            });

            m_eventManager.Subscribe<ScientificVictory>(eventObj =>
            {
                if (m_scienceGameOverScreen is not null && m_sciencePlayerName is not null)
                {
                    m_sciencePlayerName.Text = $"{eventObj.PlayerName}";
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
                    m_citizenSecondPlayerBlueCardNumber is not null)
                {
                    m_citizenFirstPlayerName.Text = eventObj.FirstPlayer.name;
                    m_citizenFirstPlayerVictoryPoints.Text = eventObj.FirstPlayer.victoryPoints.ToString();
                    m_citizenFirstPlayerBlueCardNumber.Text = eventObj.FirstPlayer.numberOfBlueCards.ToString();

                    m_citizenSecondPlayerName.Text = eventObj.SecondPlayer.name;
                    m_citizenSecondPlayerVictoryPoints.Text = eventObj.SecondPlayer.victoryPoints.ToString();
                    m_citizenSecondPlayerBlueCardNumber.Text = eventObj.SecondPlayer.numberOfBlueCards.ToString();


                    string resultText = string.Empty;
                    if (eventObj.FirstPlayer.victoryPoints > eventObj.SecondPlayer.victoryPoints)
                    {
                        resultText = "Győzelem!";
                        m_citizenFirstPlayerName.Bold = true;
                    }
                    else if (eventObj.FirstPlayer.victoryPoints < eventObj.SecondPlayer.victoryPoints)
                    {
                        resultText = "Győzelem!";
                        m_citizenSecondPlayerName.Bold = true;
                    }
                    else
                    {
                        if (eventObj.FirstPlayer.numberOfBlueCards > eventObj.SecondPlayer.numberOfBlueCards)
                        {
                            resultText = "Győzelem!";
                            m_citizenFirstPlayerName.Bold = true;
                        }
                        else if (eventObj.FirstPlayer.numberOfBlueCards < eventObj.SecondPlayer.numberOfBlueCards)
                        {
                            resultText = "Győzelem!";
                            m_citizenSecondPlayerName.Bold = true;
                        }
                        else
                        {
                            resultText = "Döntetlen!";
                        }
                    }

                    m_citizenGameResult.Text = resultText;
                    m_citizenGameOverScreen.Visible = true;
                }
            });
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
        private GraphicsLayer? m_loadingScreen;
        private GraphicsLayer? m_scienceGameOverScreen;
        private GraphicsLayer? m_militaryGameOverScreen;
        private GraphicsLayer? m_citizenGameOverScreen;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEventManager m_eventManager;
    }
}
