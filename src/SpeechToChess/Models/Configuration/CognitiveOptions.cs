using System.ComponentModel.DataAnnotations;

namespace SpeechToChess.Models.Configuration
{
    public class CognitiveOptions
    {
        public const string Cognitive = "Cognitive";

        [Required]
        public string Key { get; set; } = string.Empty;

        [Required]
        public string Region { get; set; } = string.Empty;

        [Required]
        public string EndpointId { get; set; } = string.Empty;
    }
}
