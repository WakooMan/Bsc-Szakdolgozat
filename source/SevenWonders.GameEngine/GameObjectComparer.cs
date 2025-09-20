namespace SevenWonders.GameEngine
{
    public class GameObjectComparer : IComparer<GameObject>
    {
        public int Compare(GameObject? x, GameObject? y)
        {
            if (x is null || y is null)
            {
                throw new ArgumentNullException("One of the argument is a null reference!");
            }

            return x.Zindex.CompareTo(y.Zindex);
        }
    }
}
