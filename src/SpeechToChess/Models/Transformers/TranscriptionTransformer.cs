namespace SpeechToChess.Models.Transformers
{
    public class TranscriptionTransformer : ICommandTransformer
    {
        private ICommandTransformer[] _transformers = new ICommandTransformer[]
        {
            new LowercaseCommandTransfomer(),
            new PunctuationCommandTransformer(),
            new ReverseAdvancedFileCommandTransformer(),
            new ReverseSimpleFileCommandTransformer(),
            new ReverseNumberCommandTransfomer(),
        };

        public string Apply(string input)
        {
            for (int i = 0; i < _transformers.Length; i++)
            {
                input = _transformers[i].Apply(input);
            }

            return input;
        }
    }
}
