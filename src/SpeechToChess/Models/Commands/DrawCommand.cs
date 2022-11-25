namespace SpeechToChess.Models.Commands
{
    // Supports:
    // draw
    internal class DrawCommand : ICommand
    {
        public string Text => "clock";

        public static bool TryParse(string[] parts, out ICommand? drawCommand)
        {
            if (parts.Length != 1)
            {
                drawCommand = null;
                return false;
            }

            if (parts[0] == "draw")
            {
                drawCommand = new DrawCommand();
                return true;
            }

            drawCommand = null;
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(DrawCommand)}";
        }
    }
}
