namespace SpeechToChess.Models.Cognitive
{
    public class NBest
    {
        public float Confidence { get; set; }
        public string Lexical { get; set; }
        public string ITN { get; set; }
        public string MaskedITN { get; set; }
        public string Display { get; set; }
        public IEnumerable<WordDetails> WordDetails { get; set; }
    }
}
