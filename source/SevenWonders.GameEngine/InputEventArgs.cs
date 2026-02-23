using SevenWonders.Common;

namespace SevenWonders.GameEngine
{
    public class InputEventArgs: EventArgs
    {
        public InputEventArgs()
        {
            arguments = new Dictionary<string, object>();
        }

        public void AddArgument<T>(string name, T obj)
        {
            ArgumentChecker.CheckNull(name, nameof(name));
            ArgumentChecker.CheckNull(obj, nameof(obj));

            arguments[name] = obj;
        }

        public T? GetArgument<T>(string name)
        {
            if (arguments.TryGetValue(name, out object? obj) && obj is not null)
            {
                if (obj is T value)
                {
                    return value;
                }
            }

            return default;
        }

        private readonly Dictionary<string, object> arguments;
    }
}
