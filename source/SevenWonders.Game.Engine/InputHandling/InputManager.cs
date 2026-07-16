using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Collections.Concurrent;

namespace SevenWonders.Game.Engine.InputHandling
{
    public class InputManager : IInputManager
    {
        public InputManager()
        {
            m_TouchEvents = new ConcurrentDictionary<TouchEvent, ConcurrentDictionary<SKMouseButton, ConcurrentDictionary<Action<SKTouchEventArgs>, int>>>();
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
                        foreach (var action in actions)
                        {
                            action.Key(touchEventArgs);
                        }
                    }
                }
            }
        }

        public void SubscribeTouchEvent(TouchEvent touchEvent, SKMouseButton mouseButton, Action<SKTouchEventArgs> action)
        {
            if (!m_TouchEvents.ContainsKey(touchEvent))
            {
                m_TouchEvents[touchEvent] = new ConcurrentDictionary<SKMouseButton, ConcurrentDictionary<Action<SKTouchEventArgs>, int>>();
            }
            if (!m_TouchEvents[touchEvent].ContainsKey(mouseButton))
            {
                m_TouchEvents[touchEvent][mouseButton] = new ConcurrentDictionary<Action<SKTouchEventArgs>, int>();
            }
            m_TouchEvents[touchEvent][mouseButton].TryAdd(action, 0);
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
            m_TouchEvents[touchEvent][mouseButton].TryRemove(action, out _);
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
                    return duration < TapThresholdMs && distance < MoveThreshold ?
                        TouchEvent.Clicked | TouchEvent.Released : 
                        TouchEvent.Released;
                case SKTouchAction.Moved:
                    return TouchEvent.Moved;
                default:
                    return TouchEvent.Unknown;
            }
        }

        private readonly ConcurrentDictionary<TouchEvent, ConcurrentDictionary<SKMouseButton, ConcurrentDictionary<Action<SKTouchEventArgs>, int>>> m_TouchEvents;
        private long m_touchStartTime;
        private SKPoint m_touchStartPoint;
        private const int TapThresholdMs = 500;
        private const int MoveThreshold = 10;

    }
}
