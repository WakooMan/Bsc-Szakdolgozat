namespace SevenWondersUI
{
    public partial class App : Application
    {
        public App(AppShell shell)
        {
            InitializeComponent();
            m_shell = shell;
            MainPage = m_shell;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(m_shell);
        }

        private readonly AppShell m_shell;
    }
}