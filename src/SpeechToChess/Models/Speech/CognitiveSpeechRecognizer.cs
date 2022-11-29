using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Options;
using SpeechToChess.Models.Configuration;

namespace SpeechToChess.Models.Speech
{
    public class CognitiveSpeechRecognizer : ISpeechRecognizer
    {
        private bool _disposed;
        private SpeechRecognizer _speechRecognizer;

        public event EventHandler<SpeechRecognitionEventArgs>? Recognized;

        public CognitiveSpeechRecognizer(IOptions<CognitiveOptions> options)
        {
            string key = options.Value.Key;
            string region = options.Value.Region;

            SpeechConfig speechConfig = SpeechConfig.FromSubscription(key, region);
            speechConfig.EndpointId = options.Value.EndpointId;

            AudioConfig audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            _speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);
            _speechRecognizer.Recognized += OnSpeechRecognized;
        }

        public void Load()
        {
            // No-op
        }

        public void SetInputToDefaultAudioDevice()
        {
        }

        public async Task StartContinuousRecognitionAsync()
        {
            await _speechRecognizer.StartContinuousRecognitionAsync();
        }

        public async Task TaskStopContinuousRecognitionAsync()
        {
            await _speechRecognizer.StopContinuousRecognitionAsync();
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void OnSpeechRecognized(object? sender, Microsoft.CognitiveServices.Speech.SpeechRecognitionEventArgs e)
        {
            if (e.Result.Reason == ResultReason.RecognizedSpeech)
            {
                Recognized?.Invoke(this, new SpeechRecognitionEventArgs(e.Result.Text));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _speechRecognizer.Dispose();
            }
            _disposed = true;
        }
    }
}
