using System.Text.RegularExpressions;

namespace SpeechToChess.Models.Transformers
{
    public class ReverseNumberCommandTransfomer : ICommandTransformer
    {
        private static Dictionary<string, string> NumberMap = new Dictionary<string, string>()
        {
            { "1", "one"   },
            { "2", "two"   },
            { "3", "three" },
            { "4", "four"  },
            { "5", "five"  },
            { "6", "six"   },
            { "7", "seven" },
            { "8", "eight" },
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
