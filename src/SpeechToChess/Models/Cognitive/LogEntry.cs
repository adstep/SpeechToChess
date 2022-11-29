namespace SpeechToChess.Models.Cognitive
{
    public class LogEntry
    {
        public Stream AudioStream { get; set; }
        public RecognitionResults RecognitionResults { get; set; }
    }
}
