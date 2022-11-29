using SpeechToChess.Utilities.Converters;
using System.Text.Json.Serialization;

namespace SpeechToChess.Models.Cognitive
{
    public class RecognitionResult
    {
        public string RecognitionStatus { get; set; }
        public int Channel { get; set; }
        public int? Speaker { get; set; }
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Offset { get; set; }
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Duration { get; set; }
        public long OffsetInTicks { get; set; }
        public long DurationInTicks { get; set; }
        public IEnumerable<NBest> NBest { get; set; }
    }
}
