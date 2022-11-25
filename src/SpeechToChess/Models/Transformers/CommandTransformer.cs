namespace SpeechToChess.Models.Transformers
{
    internal class CommandTransformer : ICommandTransformer
    {
        private ICommandTransformer[] _transformers = new ICommandTransformer[]
        {
            new LowercaseCommandTransfomer(),
            new NumberCommandTransfomer(),
            new SimpleFileCommandTransformer(),
            new AdvancedFileCommandTransformer(),
            new PunctuationCommandTransformer(),
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
