using System.Text.RegularExpressions;

namespace SpeechToChess.Models.Transformers
{
    public class PunctuationCommandTransformer : ICommandTransformer
    {
        public string Apply(string input)
        {
            // Remove any punctuation
            return Regex.Replace(input, @"[,\.\?]", "");
        }
    }
}
