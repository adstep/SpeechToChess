using SpeechToChess.Models.Chess;

namespace SpeechToChess.Models.Commands
{
    // Supports:
    // move {piece} to {file}{rank}
    // {piece} [moves] to {file}{rank}
    // {piece} {file}{rank}
    public class PieceMoveCommand : ICommand
    {
        public static Dictionary<Piece, string> PieceMap = new Dictionary<Piece, string>()
        {
            { Piece.King,   "K" },
            { Piece.Queen,  "Q" },
            { Piece.Bishop, "B" },
            { Piece.Knight, "N" },
            { Piece.Rook,   "R" },
            { Piece.Pawn,   "P" },
        };

        public Piece Piece { get; set; }
        public Coordinate To { get; set; }

        public string Text => $"{PieceMap[Piece]}{To}";

        public PieceMoveCommand(Piece piece, Coordinate to)
        {
            Piece = piece;
            To = to;
        }

        public static bool TryParse(string[] parts, out ICommand? pieceMoveCommand)
        {
            if (TryParseMovePieceToCoordinate(parts, out pieceMoveCommand))
            {
                return true;
            }

            if (TryParsePieceMovesToCoordinate(parts, out pieceMoveCommand))
            {
                return true;
            }

            if (TryParsePieceCoordinate(parts, out pieceMoveCommand))
            {
                return true;
            }

            pieceMoveCommand = null;
            return false;
        }

        private static bool TryParseMovePieceToCoordinate(string[] parts, out ICommand? pieceMoveCommand)
        {
            if (parts.Length != 4)
            {
                pieceMoveCommand = null;
                return false;
            }

            if (parts[0] != "move")
            {
                pieceMoveCommand = null;
                return false;
            }

            if (!Enum.TryParse(parts[1], true, out Piece piece))
            {
                pieceMoveCommand = null;
                return false;
            }

            if (parts[2] != "to")
            {
                pieceMoveCommand = null;
                return false;
            }

            if (!Coordinate.TryParse(parts[3], out Coordinate? to))
            {
                pieceMoveCommand = null;
                return false;
            }

            pieceMoveCommand = new PieceMoveCommand(piece, to!);
            return true;
        }

        private static bool TryParsePieceMovesToCoordinate(string[] parts, out ICommand? pieceMoveCommand)
        {
            if (parts.Length < 3 || parts.Length > 4)
            {
                pieceMoveCommand = null;
                return false;
            }

            if (!Enum.TryParse(parts[0], true, out Piece piece))
            {
                pieceMoveCommand = null;
                return false;
            }

            bool isMovesTo = parts[1] == "moves" && parts[2] == "to";
            bool isTo = parts[1] == "to";

            if (!isMovesTo && !isTo)
            {
                pieceMoveCommand = null;
                return false;
            }

            int coordinateIndex = isMovesTo ? 3 : 2;

            if (!Coordinate.TryParse(parts[coordinateIndex], out Coordinate? to))
            {
                pieceMoveCommand = null;
                return false;
            }

            pieceMoveCommand = new PieceMoveCommand(piece, to!);
            return true;
        }

        private static bool TryParsePieceCoordinate(string[] parts, out ICommand? pieceMoveCommand)
        {
            if (parts.Length != 2)
            {
                pieceMoveCommand = null;
                return false;
            }

            if (!Enum.TryParse(parts[0], true, out Piece piece))
            {
                pieceMoveCommand = null;
                return false;
            }

            if (!Coordinate.TryParse(parts[1], out Coordinate? to))
            {
                pieceMoveCommand = null;
                return false;
            }

            pieceMoveCommand = new PieceMoveCommand(piece, to!);
            return true;
        }

        public override string ToString()
        {
            return $"{nameof(PieceMoveCommand)} {PieceMap[Piece]}{To}";
        }
    }
}
