using System.Runtime.CompilerServices;

namespace SevenWonders.Common
{
    public static class ArgumentChecker
    {
        public static void CheckNull(object? obj, string argumentName, [CallerMemberName] string callerName = "")
        {
            if (obj is null)
            {
                throw new ArgumentNullException($"[{callerName}] - Argument with name {argumentName} cannot be null!");
            }
        }

        public static void CheckNullOrEmpty(string? str, string argumentName, [CallerMemberName] string callerName = "")
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException($"[{callerName}] - Argument with name {argumentName} cannot be null!");
            }
        }

        public static void CheckPredicateForArgument(Func<bool> predicate, string message, [CallerMemberName] string callerName = "")
        {
            if (predicate())
            {
                throw new ArgumentException($"[{callerName}] - {message}");
            }
        }

        public static void CheckPredicateForOperation(Func<bool> predicate, string message, [CallerMemberName] string callerName = "")
        {
            if (predicate())
            {
                throw new InvalidOperationException($"[{callerName}] - {message}");
            }
        }
    }
}
