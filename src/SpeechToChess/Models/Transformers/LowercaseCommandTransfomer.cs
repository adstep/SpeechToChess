namespace SpeechToChess.Models.Transformers
{
    public class LowercaseCommandTransfomer : ICommandTransformer
    {
        public string Apply(string input)
        {
            return input.ToLowerInvariant();
        }
    }
}
