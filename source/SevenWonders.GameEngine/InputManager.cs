using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace SevenWonders.GameEngine
{
    public class InputManager : IInputManager
    {
        public InputManager()
        {
            m_TouchEvents = new Dictionary<TouchEvent, Dictionary<SKMouseButton, List<Action<SKTouchEventArgs>>>>();
        }

        public void OnTouchEvent(SKTouchEventArgs touchEventArgs)
        {
            TouchEvent touchEventFlags = HandleTouchAction(touchEventArgs);
            foreach (TouchEvent touchEvent in touchEventFlags.GetFlagsBitwise())
            {
                if (m_TouchEvents.TryGetValue(touchEvent, out var eventsDictionary))
                {
                    if (eventsDictionary.TryGetValue(touchEventArgs.MouseButton, out var actions))
                    {
                        actions.ForEach(action => action(touchEventArgs));
                    }
                }
            }
        }

        public void SubscribeTouchEvent(TouchEvent touchEvent, SKMouseButton mouseButton, Action<SKTouchEventArgs> action)
        {
            if (!m_TouchEvents.ContainsKey(touchEvent))
            {
                m_TouchEvents[touchEvent] = new Dictionary<SKMouseButton, List<Action<SKTouchEventArgs>>>();
            }
            if (!m_TouchEvents[touchEvent].ContainsKey(mouseButton))
            {
                m_TouchEvents[touchEvent][mouseButton] = new List<Action<SKTouchEventArgs>>();
            }

            m_TouchEvents[touchEvent][mouseButton].Add(action);
        }

        public void UnsubscribeTouchEvent(TouchEvent touchEvent, SKMouseButton mouseButton, Action<SKTouchEventArgs> action)
        {
            if (!m_TouchEvents.ContainsKey(touchEvent))
            {
                return;
            }
            if (!m_TouchEvents[touchEvent].ContainsKey(mouseButton))
            {
                return;
            }

            m_TouchEvents[touchEvent][mouseButton].Remove(action);
        }

        private TouchEvent HandleTouchAction(SKTouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.ActionType)
            {
                case SKTouchAction.Pressed:
                    m_touchStartTime = DateTime.Now.Ticks;
                    m_touchStartPoint = touchEventArgs.Location;
                    return TouchEvent.Pressed;
                case SKTouchAction.Released:
                    long duration = (DateTime.Now.Ticks - m_touchStartTime) / TimeSpan.TicksPerMillisecond;
                    float distance = SKPoint.Distance(m_touchStartPoint, touchEventArgs.Location);
                    return (duration < TapThresholdMs && distance < MoveThreshold) ?
                        TouchEvent.Clicked | TouchEvent.Released : 
                        TouchEvent.Released;
                case SKTouchAction.Moved:
                    return TouchEvent.Moved;
                default:
                    return TouchEvent.Unknown;
            }
        }

        private readonly Dictionary<TouchEvent, Dictionary<SKMouseButton, List<Action<SKTouchEventArgs>>>> m_TouchEvents;
        private long m_touchStartTime;
        private SKPoint m_touchStartPoint;
        private const int TapThresholdMs = 500;
        private const int MoveThreshold = 10;

    }
}
