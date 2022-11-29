namespace SpeechToChess.Models.Cognitive
{
    public class CombinedResults
    {
        public int Channel { get; set; }
        public string Lexical { get; set; }
        public string ITN { get; set; }
        public string MaskedITN { get; set; }
        public string Display { get; set; }
    }
}
