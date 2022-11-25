namespace SpeechToChess.Models.Commands
{
    public class ClearCommand : ICommand
    {
        public string Text => "clear";

        public static bool TryParse(string[] parts, out ICommand? clearCommand)
        {
            if (parts.Length != 1)
            {
                clearCommand = null;
                return false;
            }

            if (parts[0] != "clear")
            {
                clearCommand = null;
                return false;
            }

            clearCommand = new ClearCommand();
            return true;
        }

        public override string ToString()
        {
            return $"{nameof(ClearCommand)}";
        }
    }
}
