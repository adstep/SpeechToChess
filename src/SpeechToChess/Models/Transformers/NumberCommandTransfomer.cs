using System.Text.RegularExpressions;

namespace SpeechToChess.Models.Transformers
{
    public class NumberCommandTransfomer : ICommandTransformer
    {
        private static Dictionary<string, string> NumberMap = new Dictionary<string, string>()
        {
            { "one",   "1" },
            { "two",   "2" },
            { "three", "3" },
            { "four",  "4" },
            { "five",  "5" },
            { "six",   "6" },
            { "seven", "7" },
            { "eight", "8" },
        };

        public string Apply(string input)
        {
            // Simplify references to numbers
            foreach (var numberMapping in NumberMap)
            {
                input = Regex.Replace(input, numberMapping.Key, numberMapping.Value);
            }

            return input;
        }
    }
}
