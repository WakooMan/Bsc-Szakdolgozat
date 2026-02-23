namespace SevenWonders.GameEngine
{
    public class GameEngineConfiguration
    {
        public GameEngineConfiguration(int fps)
        {
            FPS = fps;
        }

        public int FPS 
        { 
            get 
            {
                return m_fps;
            } 
            set 
            {
                m_fps = value;
                TargetFrameTime = 1000.0 / m_fps;
            }
        }

        public double TargetFrameTime { get; private set; }

        private int m_fps;
    }
}