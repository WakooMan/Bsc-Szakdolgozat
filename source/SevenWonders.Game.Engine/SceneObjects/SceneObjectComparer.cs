namespace SevenWonders.Game.Engine.SceneObjects
{
    public class SceneObjectComparer : IComparer<SceneObject>
    {
        public int Compare(SceneObject? x, SceneObject? y)
        {
            if (x is null || y is null)
            {
                throw new ArgumentNullException("One of the argument is a null reference!");
            }

            return x.ZIndex.CompareTo(y.ZIndex);
        }
    }
}
