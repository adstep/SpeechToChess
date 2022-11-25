namespace SpeechToChess.Models.Commands
{
    public class WhoCommand : ICommand
    {
        public string Text => "who";

        public static bool TryParse(string[] parts, out ICommand? command)
        {
            if (parts.Length != 1)
            {
                command = null;
                return false;
            }

            if (parts[0] == "who")
            {
                command = new WhoCommand();
                return true;
            }

            command = null;
            return false;
        }

        public override string ToString()
        {
            return $"${nameof(WhoCommand)}";
        }
    }
}
