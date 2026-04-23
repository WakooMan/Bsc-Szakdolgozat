using SevenWonders.AI.Model.AIModelHandler;

namespace SevenWondersUI
{
    public partial class App : Application
    {
        public App(AppShell shell, IAIModelHandler aIModelHandler)
        {
            InitializeComponent();
            m_aIModelHandler = aIModelHandler;
            m_shell = shell;
            MainPage = m_shell;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(m_shell);
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await m_aIModelHandler.Initialize();
        }

        private readonly AppShell m_shell;
        private readonly IAIModelHandler m_aIModelHandler;
    }
}