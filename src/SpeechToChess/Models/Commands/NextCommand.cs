namespace SpeechToChess.Models.Commands
{
    public class NextCommand : ICommand
    {
        public string Text => "next";

        public static bool TryParse(string[] parts, out ICommand? command)
        {
            if (parts.Length != 1)
            {
                command = null;
                return false;
            }

            if (parts[0] == "next")
            {
                command = new NextCommand();
                return true;
            }

            command = null;
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(NextCommand)}";
        }
    }
}
