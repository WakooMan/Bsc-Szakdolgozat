namespace SevenWonders.GameEngine
{
    public static class Extensions
    {
        public static IEnumerable<TouchEvent> GetFlagsBitwise(this TouchEvent value)
        {
            foreach (TouchEvent flag in Enum.GetValues(typeof(TouchEvent)))
            {
                if (flag != TouchEvent.Unknown && (value & flag) == flag)
                {
                    yield return flag;
                }
            }
        }
    }
}
