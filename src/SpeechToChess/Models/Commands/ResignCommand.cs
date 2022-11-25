namespace SpeechToChess.Models.Commands
{
    public class ResignCommand : ICommand
    {
        public string Text => "resign";

        public static bool TryParse(string[] parts, out ICommand? resignCommand)
        {
            if (parts.Length != 1)
            {
                resignCommand = null;
                return false;
            }

            if (parts[0] != "resign")
            {
                resignCommand = null;
                return false;
            }

            resignCommand = new ResignCommand();
            return true;
        }

        public override string ToString()
        {
            return $"{nameof(ResignCommand)}";
        }
    }
}
