using SpeechToChess.Utilities.Converters;
using System.Text.Json.Serialization;

namespace SpeechToChess.Models.Cognitive
{
    public class RecognitionResults
    {
        public string Source { get; set; }
        public string RequestId { get; set; }
        public DateTime Timestamp { get; set; }
        public long DurationInTicks { get; set; }
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Duration { get; set; }
        public List<CombinedResults> CombinedRecognizedPhrases { get; set; }
        public List<RecognitionResult> RecognizedPhrases { get; set;}
    }
}
