using System.Text.RegularExpressions;

namespace SpeechToChess.Models.Transformers
{
    public class AdvancedFileCommandTransformer : ICommandTransformer
    {
        public static List<string> AdvancedFiles = new List<string>()
        {
            "alpha",
            "bravo",
            "charlie",
            "delta",
            "echo",
            "foxtrot",
            "golf",
            "hotel"
        };

        public string Apply(string input)
        {
            // Simply advanced files (alpha, bravo, charlie, ...)
            foreach (var file in AdvancedFiles)
            {
                input = Regex.Replace(input, $" ({file}) ([0-9]+)", " $1$2");
            }

            return input;
        }
    }
}
