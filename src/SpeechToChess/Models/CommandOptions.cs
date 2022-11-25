using CommandLine;

namespace SpeechToChess.Models
{
    public enum RecognitionEngineType
    {
        Local,
        Cognitive
    }

    public class CommandOptions
    {
        [Option('e', "engine", Default = RecognitionEngineType.Local, HelpText = "Set speech recognition engine.")]        
        public RecognitionEngineType Engine { get; set; }
    }
}
