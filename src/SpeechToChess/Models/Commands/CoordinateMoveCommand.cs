using SpeechToChess.Models.Chess;

namespace SpeechToChess.Models.Commands
{
    // Supports
    // move {file}{rank} to {file}{rank}
    // {file}{rank} [moves] to {file}{rank}
    // {file}{rank}
    public class CoordinateMoveCommand : ICommand
    {
        public Coordinate? From { get; set; }
        public Coordinate To { get; set; }

        public string Text => $"{From?.ToString()}{To}";

        public CoordinateMoveCommand(Coordinate? from, Coordinate to)
        {
            From = from;
            To = to;
        }   
       
        public static bool TryParse(string[] parts, out ICommand? moveCommand)
        {
            if (TryParseMoveCoordinateToCoordinate(parts, out moveCommand))
            {
                return true;
            }

            if (TryParseCoordinateMovesToCoordinate(parts, out moveCommand))
            {
                return true;
            }

            if (TryParseCoordinate(parts, out moveCommand))
            {
                return true;
            }

            moveCommand = null;
            return false;
        }

        private static bool TryParseMoveCoordinateToCoordinate(string[] parts, out ICommand? moveCommand)
        {
            if (parts.Length != 4)
            {
                moveCommand = null;
                return false;
            }

            if (parts[0] != "move")
            {
                moveCommand = null;
                return false;
            }

            if (!Coordinate.TryParse(parts[1], out Coordinate? from))
            {
                moveCommand = null;
                return false;
            }

            if (parts[2] != "to")
            {
                moveCommand = null;
                return false;
            }

            if (!Coordinate.TryParse(parts[3], out Coordinate? to))
            {
                moveCommand = null;
                return false;
            }

            moveCommand = new CoordinateMoveCommand(from, to);
            return true;
        }

        private static bool TryParseCoordinateMovesToCoordinate(string[] parts, out ICommand? moveCommand)
        {
            if (parts.Length < 3 || parts.Length > 4)
            {
                moveCommand = null;
                return false;
            }

            if (!Coordinate.TryParse(parts[0], out Coordinate? from))
            {
                moveCommand = null;
                return false;
            }

            bool isMovesTo = parts[1] == "moves" && parts[2] == "to";
            bool isTo = parts[1] == "to";

            if (!isMovesTo && !isTo)
            {
                moveCommand = null;
                return false;
            }

            int coordinateIndex = isMovesTo ? 3 : 2;

            if (!Coordinate.TryParse(parts[coordinateIndex], out Coordinate? to))
            {
                moveCommand = null;
                return false;
            }

            moveCommand = new CoordinateMoveCommand(from, to);
            return true;
        }

        private static bool TryParseCoordinate(string[] parts, out ICommand? moveCommand)
        {
            if (parts.Length != 1)
            {
                moveCommand = null;
                return false;
            }

            if (!Coordinate.TryParse(parts[0], out Coordinate? to))
            {
                moveCommand = null;
                return false;
            }

            moveCommand = new CoordinateMoveCommand(null, to);
            return true;
        }

        public override string ToString()
        {
            return $"{nameof(CoordinateMoveCommand)} {From}{To}";
        }
    }
}
