namespace SpeechToChess.Models.Speech
{
    public class SpeechRecognitionEventArgs
    {
        public string Text { get; }

        public SpeechRecognitionEventArgs(string text) { Text = text; } 
    }

    public interface ISpeechRecognizer : IDisposable
    {
        event EventHandler<SpeechRecognitionEventArgs> Recognized;

        void Load();
        void SetInputToDefaultAudioDevice();

        Task StartContinuousRecognitionAsync();
        Task TaskStopContinuousRecognitionAsync();
    }
}
