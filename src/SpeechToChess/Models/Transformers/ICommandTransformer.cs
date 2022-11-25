namespace SpeechToChess.Models.Transformers
{
    public interface ICommandTransformer
    {
        string Apply(string input);
    }
}
