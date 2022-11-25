using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SpeechToChess.Models.Commands
{
    public class CommandResolver : ICommandResolver
    {
        public Dictionary<Type, MethodInfo> ResolverMap = new Dictionary<Type, MethodInfo>();
        public List<string> Resolvers => ResolverMap.Keys.Select(t => t.Name).ToList();

        public void LoadCommands()
        {
            Dictionary<Type, MethodInfo> commands = new Dictionary<Type, MethodInfo>();

            Assembly assembly = Assembly.GetExecutingAssembly();

            Type commandResolverType = typeof(CommandResolver);

            Type[] types = new Type[] { typeof(string[]), typeof(ICommand) };
            MethodInfo? tryResolveMethodInfo = commandResolverType.GetMethod(nameof(CommandResolver.TryParse), BindingFlags.NonPublic | BindingFlags.Static);

            if (tryResolveMethodInfo == null)
            {
                throw new InvalidOperationException($"Unable to find TryParse method on CommandResolver.");
            }

            foreach (Type type in assembly.GetTypes())
            {
                if (type == null)
                    continue;

                if (typeof(ICommand).IsAssignableFrom(type) && type.IsClass)
                {
                    MethodInfo? methodInfo = type.GetMethod(nameof(ICommand.TryParse), BindingFlags.Public | BindingFlags.Static);

                    if (methodInfo == null)
                    {
                        continue;
                    }

                    methodInfo = tryResolveMethodInfo.MakeGenericMethod(type);
                    ResolverMap.Add(type, methodInfo);
                }
            }
        }

        public bool TryResolve(string text, [NotNullWhen(true)] out ICommand? command)
        {
            string[] parts = text.Split(' ');

            foreach (KeyValuePair<Type, MethodInfo> resolver in ResolverMap)
            {
                Type type = resolver.Key;
                MethodInfo methodInfo = resolver.Value;

                object?[] parameters = new object?[] { parts, null };

                bool? result = methodInfo.Invoke(null, parameters) as bool?;

                if (result.HasValue && result.Value)
                {
                    command = (ICommand)parameters[1]!;
                    return true;
                }
            }

            command = null;
            return false;
        }

        private static bool TryParse<T>(string[] parts, out ICommand? command) where T : ICommand
        {
            T.TryParse(parts, out command);
            return command != null;
        }
    }
}
