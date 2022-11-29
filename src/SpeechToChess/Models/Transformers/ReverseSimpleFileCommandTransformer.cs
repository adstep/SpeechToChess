using System.Text.RegularExpressions;

namespace SpeechToChess.Models.Transformers
{
    public class ReverseSimpleFileCommandTransformer : ICommandTransformer
    {
        private static List<string> SimpleFiles = new List<string>()
        {
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h"
        };

        public string Apply(string input)
        {
            // Simplify simple files (a, b, c, ...)
            foreach (var file in SimpleFiles)
            {
                input = Regex.Replace(input, $"({file})([0-9]+)", "$1 $2");
            }

            return input;
        }
    }
}
