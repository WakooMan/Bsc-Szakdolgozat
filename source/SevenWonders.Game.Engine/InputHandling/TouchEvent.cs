namespace SevenWonders.Game.Engine.InputHandling
{
    [Flags]
    public enum TouchEvent
    {
        Unknown  = 0,
        Pressed  = 1,
        Released = 2,
        Clicked  = 4,
        Moved    = 8,
    }
}
