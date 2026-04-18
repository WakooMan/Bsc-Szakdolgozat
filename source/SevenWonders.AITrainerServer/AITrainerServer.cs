using GameLogic;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace SevenWonders.AITrainerServer
{
    public class AITrainerServer
    {
        private TcpListener? m_server;
        private readonly IGame m_game;

        public AITrainerServer(IGame game)
        {
            m_game = game;
        }

        public void StartServer()
        {
            m_server = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
            m_server.Start();
            Console.WriteLine("Waiting for Python AI connection on port 5000...");

            using (TcpClient client = m_server.AcceptTcpClient())
            using (NetworkStream stream = client.GetStream())
            {
                Console.WriteLine("Python AI joined!");
                byte[] buffer = new byte[4096];

                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (message == "RESET")
                    {
                        m_game.ResetGame();
                        SendState(stream);
                    }
                    else if (int.TryParse(message, out int action))
                    {
                        float reward = m_game.ExecuteAction(action);
                        bool isDone = m_game.IsGameOver();

                        SendState(stream, reward, isDone);

                        if (isDone) m_game.ResetGame();
                    }
                }
            }
        }

        private void SendState(NetworkStream stream, float reward = 0, bool done = false)
        {
            float[] state = m_game.GetStateVector();

            string stateString = "[" + string.Join(",", state) + "]";
            string response = $"{stateString}|{reward}|{done}";

            byte[] data = Encoding.UTF8.GetBytes(response);
            stream.Write(data, 0, data.Length);
        }
    }
}