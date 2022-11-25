using SpeechToChess.Models.Chess;

namespace SpeechToChess.Models.Commands
{
    internal class PuzzleCommand : ICommand
    {
        public string Text => string.Empty;

        public static bool TryParse(string[] parts, out ICommand? command)
        {
            if (parts.Length != 1)
            {
                command = null;
                return false;
            }

            if (parts[0] == "puzzle")
            {
                command = new PuzzleCommand();
                return true;
            }

            command = null;
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(PuzzleCommand)}";
        }
    }
}
