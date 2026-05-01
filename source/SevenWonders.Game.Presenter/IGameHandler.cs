using SevenWonders.Common;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWonders.Game.Presenter
{
    public interface IGameHandler
    {
        void InitializeEngine();
        Task StartGame(string player1Name, PlayerType player1Type, string player2Name, PlayerType player2Type, RandomGeneratorType randomGeneratorType, int seed, int startingPlayerId, IGameOverHandler gameOverHandler);
        void StopGame();

        void Render(SKCanvas sKCanvas);
        void OnTouchEvent(SKTouchEventArgs eventArgs);
        void Resize(Vector2 newSize);

        void SubscribeRedrawRequested(EventHandler handler);
        void UnsubscribeRedrawRequested(EventHandler handler);
    }
}
