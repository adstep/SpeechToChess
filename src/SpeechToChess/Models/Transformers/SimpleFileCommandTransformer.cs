using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SpeechToChess.Models.Transformers
{
    public class SimpleFileCommandTransformer : ICommandTransformer
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
                input = Regex.Replace(input, $"({file}) ([0-9]+)", "$1$2");
            }

            return input;
        }
    }
}
