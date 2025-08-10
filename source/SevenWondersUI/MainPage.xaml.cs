using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Military;
using GameLogic.Elements.Wonders;
using SevenWonders.Common;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Diagnostics;
using System.Xml.Serialization;

namespace SevenWondersUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        float animationProgress = 0; // 0.0 → 1.0
        Stopwatch stopwatch = new();
        bool animationStarted = false;

        [Obsolete]
        public MainPage()
        {
            InitializeComponent();
            IXmlHandler xmlHandler = new XmlHandler();
            IMilitaryBoard militaryBoard = new MilitaryBoardFactory(xmlHandler).Create();
            IGameElements gameElements = new GameElements(new MainCardListFactory(xmlHandler), new WonderListFactory(xmlHandler), new DevelopmentListFactory(xmlHandler));
            var sm = gameElements.Developments;
            Task.Delay(2000).ContinueWith(_ =>
            {
                animationStarted = true;
                stopwatch.Restart();
                Device.StartTimer(TimeSpan.FromMilliseconds(16), () =>
                {
                    // 0.5 másodperc alatt fusson le az animáció
                    animationProgress = Math.Min(1f, (float)(stopwatch.Elapsed.TotalSeconds / 0.5));
                    canvas.InvalidateSurface();
                    return animationProgress < 1f;
                });
            });
        }


        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.DarkGreen);

            float cardWidth = 100;
            float cardHeight = 150;
            var center = new SKPoint(e.Info.Width / 2, e.Info.Height / 2);

            // Az animáció paraméterei
            float scale = Lerp(1.0f, 1.2f, animationProgress); // nagyítás
            float lift = Lerp(0, -50, animationProgress);      // felemelés
            float skewX = Lerp(0, -0.05f, animationProgress);  // enyhe döntés
            float shadowOffset = Lerp(5, 15, animationProgress);
            float shadowBlur = Lerp(5, 15, animationProgress);

            canvas.Save();

            // Transzformációk
            var matrix = SKMatrix.CreateIdentity();
            matrix = matrix.PostConcat(SKMatrix.CreateScale(scale, scale, center.X, center.Y));
            matrix = matrix.PostConcat(SKMatrix.CreateTranslation(0, lift));
            matrix = matrix.PostConcat(SKMatrix.CreateSkew(skewX, 0));
            canvas.SetMatrix(matrix);

            // Árnyék
            using (var shadowPaint = new SKPaint
            {
                Color = SKColors.Black.WithAlpha(80),
                ImageFilter = SKImageFilter.CreateBlur(shadowBlur, shadowBlur)
            })
            {
                canvas.DrawRoundRect(center.X - cardWidth / 2 + shadowOffset,
                                     center.Y - cardHeight / 2 + shadowOffset,
                                     cardWidth, cardHeight, 10, 10, shadowPaint);
            }

            // Kártya
            using (var paint = new SKPaint { Color = SKColors.White, IsAntialias = true })
            {
                canvas.DrawRoundRect(center.X - cardWidth / 2,
                                     center.Y - cardHeight / 2,
                                     cardWidth, cardHeight, 10, 10, paint);
            }

            canvas.Restore();
        }

        // Segédfüggvény lineáris interpolációhoz
        private float Lerp(float start, float end, float t) => start + (end - start) * t;

    }

}
