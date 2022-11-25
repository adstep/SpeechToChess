using SpeechToChess.Models.Chess;

namespace SpeechToChess.Models.Commands
{
    public enum CastleType
    {
        Kingside,
        QueenSide 
    }

    // Supports:
    // king-side [castle]
    // queen-side [castle]
    internal class CastleCommand : ICommand
    {
        public CastleType Type { get; set; }

        public string Text => 
            (Type == CastleType.Kingside) ? 
                "O-O" : 
                (Type == CastleType.QueenSide) ?
                    "O-O-O" :
                    string.Empty;

        public CastleCommand(CastleType type)
        {
            Type = type;
        }

        public static bool TryParse(string[] parts, out ICommand? command)
        {
            if (parts.Length < 1 || parts.Length > 2)
            {
                command = null;
                return false;
            }

            if (parts.Length == 2 && parts[1] != "castle")
            {
                command = null;
                return false;
            }

            if (parts[0] == "king-side")
            {
                command = new CastleCommand(CastleType.Kingside);
                return true;
            }

            if (parts[0] == "queen-side")
            {
                command = new CastleCommand(CastleType.QueenSide);
                return true;
            }

            command = null;
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(CastleCommand)} {Type.ToString()}";
        }
    }
}
