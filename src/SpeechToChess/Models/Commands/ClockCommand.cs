namespace SpeechToChess.Models.Commands
{
    // Supports:
    // clock
    internal class ClockCommand : ICommand
    {
        public string Text => "clock";

        public static bool TryParse(string[] parts, out ICommand? clockCommand)
        {
            if (parts.Length != 1)
            {
                clockCommand = null;
                return false;
            }

            if (parts[0] == "clock")
            {
                clockCommand = new ClockCommand();
                return true;
            }

            clockCommand = null;
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(ClockCommand)}";
        }
    }
}
