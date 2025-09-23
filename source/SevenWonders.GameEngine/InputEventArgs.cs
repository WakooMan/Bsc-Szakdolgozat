namespace SevenWonders.GameEngine
{
    public class InputEventArgs: EventArgs
    {
        public InputEventArgs()
        {
            arguments = new Dictionary<string, object>();
        }

        public void AddArgument(string name, object obj)
        {
            arguments[name] = obj;
        }

        public T? GetArgument<T>(string name) where T : class
        {
            if (arguments.ContainsKey(name))
            {
                return arguments[name] as T;
            }
            return null as T;
        }

        private readonly Dictionary<string, object> arguments;
    }
}
