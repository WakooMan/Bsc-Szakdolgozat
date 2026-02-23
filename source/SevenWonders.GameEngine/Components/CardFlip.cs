namespace SevenWonders.GameEngine.Components
{
    public struct CardFlip
    {
        public CardFlip(GameObject gameObject, int spriteNumber, float flipSpeed)
        {
            GameObject = gameObject;
            SpriteNumber = spriteNumber;
            FlipSpeed = flipSpeed;
        }

        public GameObject GameObject { get; set; }
        public int SpriteNumber { get; set; }
        public float FlipSpeed { get; set; }

    }
}