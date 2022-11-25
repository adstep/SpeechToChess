namespace SpeechToChess.Models.Commands
{
    public interface ICommandResolver
    {
        List<string> Resolvers { get; }

        void LoadCommands();
        bool TryResolve(string text, out ICommand? command);
    }
}
