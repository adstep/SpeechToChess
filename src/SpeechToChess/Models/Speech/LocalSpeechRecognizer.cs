using System.Globalization;
using System.Speech.Recognition;

namespace SpeechToChess.Models.Speech
{
    public class LocalSpeechRecognizer : ISpeechRecognizer
    {
        private SpeechRecognitionEngine _speechRecognition;


        public event EventHandler<SpeechRecognitionEventArgs> Recognized;

        public LocalSpeechRecognizer()
        {
            var cultureInfo = new CultureInfo("en-US");
            _speechRecognition = new SpeechRecognitionEngine(cultureInfo);

            
            _speechRecognition.SpeechRecognized += OnSpeechRecognized;
            _speechRecognition.SetInputToDefaultAudioDevice();
        }

        public void Load()
        {
            Grammar[] grammars = GetGrammars();

            for (int i = 0; i < grammars.Length; i++)
            {
                _speechRecognition.LoadGrammar(grammars[i]);
            }
        }

        public void Dispose()
        {
        }

        public void SetInputToDefaultAudioDevice()
        {
            _speechRecognition.SetInputToDefaultAudioDevice();
        }

        public Task StartContinuousRecognitionAsync()
        {
            _speechRecognition.RecognizeAsync(RecognizeMode.Multiple);
            return Task.CompletedTask;
        }

        public Task TaskStopContinuousRecognitionAsync()
        {
            _speechRecognition.RecognizeAsyncStop();
            return Task.CompletedTask;
        }

        private void OnSpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
        {
            Recognized?.Invoke(this, new SpeechRecognitionEventArgs(e.Result.Text));
        }

        private static Grammar[] GetGrammars()
        {
            GrammarBuilder commands = new Choices(new string[]
            {
                "clear", "undo", "draw", "clock", "time", "who", "next", 
                "puzzle", "home", "resign", "draw", "quit", "close"
            });

            GrammarBuilder pieces = new Choices(new string[]
            {
                "king", "queen", "bishop", "knight", "rook", "pawn"
            });

            GrammarBuilder files = new Choices(new string[]
            {
                "a", "b", "c", "d", "e", "f", "g", "h",
            });

            GrammarBuilder ranks = new Choices(new string[]
            {
                "one", "two", "three", "four", "five", "six", "seven", "eight"
            });

            GrammarBuilder pieceCoordinate = new GrammarBuilder();
            pieceCoordinate.Append(pieces);
            pieceCoordinate.Append(files);
            pieceCoordinate.Append(ranks);

            GrammarBuilder pieceMovesToCoordinate = new GrammarBuilder();
            pieceMovesToCoordinate.Append(pieces);
            pieceMovesToCoordinate.Append(new GrammarBuilder("moves"), 0, 1);
            pieceMovesToCoordinate.Append("to");
            pieceMovesToCoordinate.Append(files);
            pieceMovesToCoordinate.Append(ranks);

            GrammarBuilder movePieceToCoordinate = new GrammarBuilder();
            movePieceToCoordinate.Append("move");
            movePieceToCoordinate.Append(pieces);
            movePieceToCoordinate.Append("to");
            movePieceToCoordinate.Append(files);
            movePieceToCoordinate.Append(ranks);

            GrammarBuilder moveCoordinateToCoordinate = new GrammarBuilder();
            moveCoordinateToCoordinate.Append("move");
            moveCoordinateToCoordinate.Append(files);
            moveCoordinateToCoordinate.Append(ranks);
            moveCoordinateToCoordinate.Append("to");
            moveCoordinateToCoordinate.Append(files);
            moveCoordinateToCoordinate.Append(ranks);

            GrammarBuilder coordinateMovesToCoordinate = new GrammarBuilder();
            coordinateMovesToCoordinate.Append(files);
            coordinateMovesToCoordinate.Append(ranks);
            coordinateMovesToCoordinate.Append(new GrammarBuilder("moves"), 0, 1);
            coordinateMovesToCoordinate.Append("to");
            coordinateMovesToCoordinate.Append(files);
            coordinateMovesToCoordinate.Append(ranks);

            GrammarBuilder coordinate = new GrammarBuilder();
            coordinate.Append(files);
            coordinate.Append(ranks);

            GrammarBuilder kingSideCastle = new GrammarBuilder();
            kingSideCastle.Append("king-side");
            kingSideCastle.Append(new GrammarBuilder("castle"), 0, 1);

            GrammarBuilder queenSideCastle = new GrammarBuilder();
            queenSideCastle.Append("queen-side");
            queenSideCastle.Append(new GrammarBuilder("castle"), 0, 1);

            GrammarBuilder promote = new GrammarBuilder();
            promote.Append(new GrammarBuilder("promote"), 0, 1);
            promote.Append(files);
            promote.Append("eight");
            promote.Append(new GrammarBuilder("to"), 0, 1);
            promote.Append(pieces);

            return new Grammar[] {
                new Grammar(commands),
                new Grammar(pieceCoordinate),
                new Grammar(pieceMovesToCoordinate),
                new Grammar(movePieceToCoordinate),
                new Grammar(moveCoordinateToCoordinate),
                new Grammar(coordinateMovesToCoordinate),
                new Grammar(coordinate),
                new Grammar(kingSideCastle),
                new Grammar(queenSideCastle),
                new Grammar(promote),
            };
        }
    }
}
