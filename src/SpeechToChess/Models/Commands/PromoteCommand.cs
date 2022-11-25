using SpeechToChess.Models.Chess;

namespace SpeechToChess.Models.Commands
{
    public class PromoteCommand : ICommand
    {
        public string Text => $"{Coordinate}={PieceMoveCommand.PieceMap[Piece]}";

        public Coordinate Coordinate { get; }
        public Piece Piece { get; }

        public PromoteCommand(Coordinate coordinate, Piece piece)
        {
            Coordinate = coordinate;
            Piece = piece;
        }

        public static bool TryParse(string[] parts, out ICommand? command)
        {
            if (TryParsePromoteFileToPiece(parts, out command))
            {
                return true;
            }

            if (TryParseFileToPiece(parts, out command))
            {
                return true;
            }

            if (TryParseFilePiece(parts, out command))
            {
                return true;
            }

            command = null;
            return false;
        }

        private static bool TryParsePromoteFileToPiece(string[] parts, out ICommand? command)
        {
            if (parts.Length != 4)
            {
                command = null;
                return false;
            }

            if (parts[0] != "promote")
            {
                command = null;
                return false;
            }

            if (!Coordinate.TryParse(parts[0], out Coordinate? coordinate))
            {
                command = null;
                return false;
            }

            if (parts[2] != "to")
            {
                command = null;
                return false;
            }

            if (!Enum.TryParse(parts[3], true, out Piece piece))
            {
                command = null;
                return false;
            }

            command = new PromoteCommand(coordinate, piece);
            return true;
        }

        private static bool TryParseFileToPiece(string[] parts, out ICommand? command)
        {
            if (parts.Length != 3)
            {
                command = null;
                return false;
            }

            if (!Coordinate.TryParse(parts[0], out Coordinate? coordinate))
            {
                command = null;
                return false;
            }

            if (parts[1] != "to")
            {
                command = null;
                return false;
            }

            if (!Enum.TryParse(parts[2], true, out Piece piece))
            {
                command = null;
                return false;
            }

            command = new PromoteCommand(coordinate, piece);
            return true;
        }

        private static bool TryParseFilePiece(string[] parts, out ICommand? command)
        {
            if (parts.Length != 2)
            {
                command = null;
                return false;
            }

            if (!Coordinate.TryParse(parts[0], out Coordinate? coordinate))
            {
                command = null;
                return false;
            }

            if (!Enum.TryParse(parts[1], true, out Piece piece))
            {
                command = null;
                return false;
            }

            command = new PromoteCommand(coordinate, piece);
            return true;
        }

        public override string ToString()
        {
            return $"{nameof(PromoteCommand)} {Coordinate}={Piece}";
        }
    }
}
