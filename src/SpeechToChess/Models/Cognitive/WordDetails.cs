using SpeechToChess.Utilities.Converters;
using System.Text.Json.Serialization;

namespace SpeechToChess.Models.Cognitive
{
    public class WordDetails
    {
        public string Word { get; set; }
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Offset { get; set; }
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Duration { get; set; }
        public double OffsetInTicks { get; set; }
        public double DurationInTicks { get; set;}
        public float Confidence { get; set; }
    }
}
