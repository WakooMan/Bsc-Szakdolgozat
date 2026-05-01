using SevenWonders.AI.Model.AIModelHandler;
using SevenWonders.AI.Model.Cache;

namespace SevenWonders.UI
{
    public partial class App : Application
    {
        public App(AppShell shell, IAIModelHandlerCache aIModelHandler)
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
            await m_aIModelHandler.EasyAIModelHandler.Initialize();
        }

        private readonly AppShell m_shell;
        private readonly IAIModelHandlerCache m_aIModelHandler;
    }
}