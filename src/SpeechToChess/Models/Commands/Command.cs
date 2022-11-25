namespace SpeechToChess.Models.Commands
{
    public interface ICommand 
    {
        string Text { get; }

        static abstract bool TryParse(string[] parts, out ICommand? command);
    }
}
