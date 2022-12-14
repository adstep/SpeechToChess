namespace SpeechToChess.Models.Commands
{
    public class CloseCommand : ICommand
    {
        public string Text => string.Empty;

        public static bool TryParse(string[] parts, out ICommand? command)
        {
            if (parts.Length != 1)
            {
                command = null;
                return false;
            }

            if (parts[0] == "quit" || parts[0] == "close")
            {
                command = new CloseCommand();
                return true;
            }

            command = null;
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(ClockCommand)}";
        }
    }
}
