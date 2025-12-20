namespace SevenWonders.GameEngine
{
    public class InputManager : IInputManager
    {
        public InputManager()
        {
            m_keyEvents = new Dictionary<KeyEvent, Dictionary<long, List<Action>>>();
            m_mouseEvents = new Dictionary<MouseEvent, Dictionary<MouseButton, List<Action<InputEventArgs>>>>();
        }

        public void KeyDown(long keyCode)
        {
            if (m_keyEvents.TryGetValue(KeyEvent.KeyDown, out var keyCodeDictionary) && keyCodeDictionary.TryGetValue(keyCode, out var actions))
            {
                actions.ForEach(action => action());
            }
        }

        public void KeyPressed(long keyCode)
        {
            if (m_keyEvents.TryGetValue(KeyEvent.KeyPressed, out var keyCodeDictionary) && keyCodeDictionary.TryGetValue(keyCode, out var actions))
            {
                actions.ForEach(action => action());
            }
        }

        public void KeyUp(long keyCode)
        {
            if (m_keyEvents.TryGetValue(KeyEvent.KeyUp, out var keyCodeDictionary) && keyCodeDictionary.TryGetValue(keyCode, out var actions))
            {
                actions.ForEach(action => action());
            }
        }

        public void MouseDown(MouseButton mouseButton, int x, int y)
        {
            InputEventArgs inputEventArgs = new InputEventArgs();
            inputEventArgs.AddArgument(nameof(x), x);
            inputEventArgs.AddArgument(nameof(y), y);

            if (m_mouseEvents.TryGetValue(MouseEvent.MouseDown, out var mouseButtonDictionary) && mouseButtonDictionary.TryGetValue(mouseButton, out var actions))
            {
                actions.ForEach(action => action(inputEventArgs));
            }
        }

        public void MouseMove(int oldX, int oldY, int newX, int newY)
        {
            InputEventArgs inputEventArgs = new InputEventArgs();
            inputEventArgs.AddArgument(nameof(oldX), oldX);
            inputEventArgs.AddArgument(nameof(oldY), oldY);
            inputEventArgs.AddArgument(nameof(newX), newX);
            inputEventArgs.AddArgument(nameof(newY), newY);

            if (m_mouseEvents.TryGetValue(MouseEvent.MouseMove, out var mouseButtonDictionary) && mouseButtonDictionary.TryGetValue(MouseButton.None, out var actions))
            {
                actions.ForEach(action => action(inputEventArgs));
            }
        }

        public void MouseUp(MouseButton mouseButton, int x, int y)
        {
            InputEventArgs inputEventArgs = new InputEventArgs();
            inputEventArgs.AddArgument(nameof(x), x);
            inputEventArgs.AddArgument(nameof(y), y);

            if (m_mouseEvents.TryGetValue(MouseEvent.MouseDown, out var mouseButtonDictionary) && mouseButtonDictionary.TryGetValue(mouseButton, out var actions))
            {
                actions.ForEach(action => action(inputEventArgs));
            }
        }

        public void MouseClicked(MouseButton mouseButton, int x, int y)
        {
            InputEventArgs inputEventArgs = new InputEventArgs();
            inputEventArgs.AddArgument(nameof(x), x);
            inputEventArgs.AddArgument(nameof(y), y);

            if (m_mouseEvents.TryGetValue(MouseEvent.MouseClicked, out var mouseButtonDictionary) && mouseButtonDictionary.TryGetValue(mouseButton, out var actions))
            {
                actions.ForEach(action => action(inputEventArgs));
            }
        }

        public void SubscribeKeyEvent(KeyEvent keyEvent, long keyCode, Action action)
        {
            if (!m_keyEvents.ContainsKey(keyEvent))
            {
                m_keyEvents[keyEvent] = new Dictionary<long, List<Action>>();
            }
            if (!m_keyEvents[keyEvent].ContainsKey(keyCode))
            {
                m_keyEvents[keyEvent][keyCode] = new List<Action>();
            }

            m_keyEvents[keyEvent][keyCode].Add(action);
        }

        public void SubscribeMouseEvent(MouseEvent mouseEvent, MouseButton mouseButton, Action<InputEventArgs> action)
        {
            if (!m_mouseEvents.ContainsKey(mouseEvent))
            {
                m_mouseEvents[mouseEvent] = new Dictionary<MouseButton, List<Action<InputEventArgs>>>();
            }
            if (!m_mouseEvents[mouseEvent].ContainsKey(mouseButton))
            {
                m_mouseEvents[mouseEvent][mouseButton] = new List<Action<InputEventArgs>>();
            }

            m_mouseEvents[mouseEvent][mouseButton].Add(action);
        }

        public void UnsubscribeMouseEvent(MouseEvent mouseEvent, MouseButton mouseButton, Action<InputEventArgs> action)
        {
            if (!m_mouseEvents.ContainsKey(mouseEvent))
            {
                return;
            }
            if (!m_mouseEvents[mouseEvent].ContainsKey(mouseButton))
            {
                return;
            }

            m_mouseEvents[mouseEvent][mouseButton].Remove(action);
        }

        private readonly Dictionary<KeyEvent, Dictionary<long, List<Action>>> m_keyEvents;
        private readonly Dictionary<MouseEvent, Dictionary<MouseButton, List<Action<InputEventArgs>>>> m_mouseEvents;

    }
}
