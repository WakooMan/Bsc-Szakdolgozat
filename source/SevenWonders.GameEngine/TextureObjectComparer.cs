namespace SevenWonders.GameEngine
{
    public class TextureObjectComparer : IComparer<TextureObject>
    {
        public int Compare(TextureObject? x, TextureObject? y)
        {
            if (x is null || y is null)
            {
                throw new ArgumentNullException("One of the argument is a null reference!");
            }

            return x.ZIndex.CompareTo(y.ZIndex);
        }
    }
}
