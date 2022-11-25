using System.ComponentModel.DataAnnotations;

namespace SpeechToChess.Models.Configuration
{
    public class LichessOptions
    {
        public const string Lichess = "Lichess";

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
