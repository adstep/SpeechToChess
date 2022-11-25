using OpenQA.Selenium.Interactions.Internal;
using System.Diagnostics.CodeAnalysis;

namespace SpeechToChess.Models.Chess
{
    public class Coordinate
    {
        public static Dictionary<string, File> FileMap = new Dictionary<string, File>()
        {
            { "a", File.A },
            { "b", File.B },
            { "c", File.C },
            { "d", File.D },
            { "e", File.E },
            { "f", File.F },
            { "g", File.G },
            { "h", File.H },
        };

        public static Dictionary<string, Rank> RankMap = new Dictionary<string, Rank>()
        {
            { "1", Rank.One   },
            { "2", Rank.Two   },
            { "3", Rank.Three },
            { "4", Rank.Four  },
            { "5", Rank.Five  },
            { "6", Rank.Six   },
            { "7", Rank.Seven },
            { "8", Rank.Eight },
        };

        public File File { get; set; }
        public Rank Rank { get; set; }

        public Coordinate(File file, Rank rank)
        {
            File = file;
            Rank = rank;
        }

        public static bool TryParseFile(string text, out File file)
        {
            if (!FileMap.ContainsKey(text))
            {
                file = File.A;
                return false;
            }

            file = FileMap[text];
            return true;
        }

        public static bool TryParse(string fileRank, [NotNullWhen(true)] out Coordinate? coordinate)
        {
            if (fileRank.Length != 2)
            {
                coordinate = null;
                return false;
            }

            char file = fileRank[0];
            char rank = fileRank[1];

            return TryParse(file.ToString(), rank.ToString(), out coordinate);
        }

        public static bool TryParse(string file, string rank, [NotNullWhen(true)] out Coordinate? coordinate)
        {
            if (!FileMap.ContainsKey(file))
            {
                coordinate = null;
                return false;
            }

            if (!RankMap.ContainsKey(rank))
            {
                coordinate = null;
                return false;
            }

            coordinate = new Coordinate(FileMap[file], RankMap[rank]);
            return true;
        }

        public override string ToString()
        {
            return $"{File}{(int)Rank + 1}".ToLower();
        }
    }
}
