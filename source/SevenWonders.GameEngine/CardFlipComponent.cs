using System.Numerics;

namespace SevenWonders.GameEngine
{
    public class CardFlipComponent : IComponent
    {
        public CardFlipComponent()
        {
            Id = 101;
            Name = nameof(CardFlipComponent);
            m_flips = new List<CardFlip>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public void HandleMessage()
        {
        }

        public void Shutdown()
        {
            m_flips.Clear();
        }

        public void Startup()
        {
            m_flips.Clear();
        }

        public void Update(float deltaTime)
        {
            List<CardFlip> flipsToRemove = new List<CardFlip>();
            foreach (CardFlip flip in m_flips)
            {
                float step = flip.FlipSpeed * deltaTime;

                float oldScale = flip.GameObject.Scale.X;
                flip.GameObject.Scale = new Vector2(flip.GameObject.Scale.X - step, flip.GameObject.Scale.Y);

                if (oldScale > 0 && flip.GameObject.Scale.X <= 0)
                {
                    flip.GameObject.CurrentAnim = flip.SpriteNumber;
                }

                if (flip.GameObject.Scale.X <= -1f)
                {
                    flip.GameObject.Scale = new Vector2(-1f, flip.GameObject.Scale.Y);
                    flipsToRemove.Add(flip);
                }
            }

            foreach (CardFlip cardFlipToRemove in flipsToRemove)
            {
                m_flips.Remove(cardFlipToRemove);
            }
        }

        public void Flip(GameObject gameObject, int spriteNumber, float flipSpeed)
        {
            lock (m_flips)
            {
                m_flips.Add(new CardFlip(gameObject, spriteNumber, flipSpeed));
            }
        }

        private readonly List<CardFlip> m_flips;
    }
}
