using Microsoft.Extensions.Options;
using SpeechToChess.Clients;
using SpeechToChess.Models.Commands;
using SpeechToChess.Models.Configuration;
using SpeechToChess.Models.Speech;
using SpeechToChess.Models.Transformers;

namespace SpeechToChess.Services
{
    public class Service : IService
    {
        private bool _isActive = false;

        private ICommandResolver _commandResolver;
        private ICommandTransformer _commandTransformer;
        private ISpeechRecognizer _speechRecognizer;
        private ILichessClient _lichessClient;
        private LichessOptions _lichessOptions;

        public Service(
            ILichessClient lichessClient,
            ISpeechRecognizer speechRecognizer,
            ICommandResolver commandResolver,
            ICommandTransformer commandTransformer,
            IOptions<LichessOptions> lichessOptions
        )
        {
            _speechRecognizer = speechRecognizer;
            _commandResolver = commandResolver;
            _commandTransformer = commandTransformer;
            _lichessClient = lichessClient;
            _lichessOptions = lichessOptions.Value;
        }

        public async Task Run()
        {
            Console.WriteLine("Loading commands...");
            _commandResolver.LoadCommands();

            foreach (string resolver in _commandResolver.Resolvers)
            {
                Console.WriteLine($"  - Loaded {resolver}");
            }

            Console.WriteLine("Loading speech recognizer...");

            _speechRecognizer.Load();

            Console.WriteLine("Initializing lichess client...");
            await _lichessClient.Init();

            Console.WriteLine("Launching browser...");

            await _lichessClient.LaunchAsync();

            Console.WriteLine("Logging in...");

            string userName = _lichessOptions.UserName;
            string password = _lichessOptions.Password;

            await _lichessClient.LoginAsync(userName, password);

            Console.WriteLine("Listening...");

            _speechRecognizer.Recognized += OnSpeechRecognized;
            await _speechRecognizer.StartContinuousRecognitionAsync();

            _isActive = true;
            while (_isActive)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            Console.WriteLine("Closing...");
        }

        private void OnSpeechRecognized(object? sender, SpeechRecognitionEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Text))
            {
                return;
            }

            var text = e.Text;

            Console.WriteLine($"  Recognized: {text}");

            text = _commandTransformer.Apply(text);

            Console.WriteLine($"  Transformed: {text}");

            if (!_commandResolver.TryResolve(text, out ICommand? command))
            {
                Console.WriteLine($"  Could not parse command: {text}");
            }
            else
            {
                Console.WriteLine($"  Parsed: {command}");

                if (command is CloseCommand)
                {
                    _isActive = false;
                    _speechRecognizer.Recognized -= OnSpeechRecognized;
                }
                else if (command is ClearCommand)
                {
                    _lichessClient.ClearInputAsync().Wait();
                }
                else if (command is PuzzleCommand)
                {
                    _lichessClient.NavigateToPuzzle().Wait();
                }
                else if (command is HomeCommand)
                {
                    _lichessClient.NavigateToHome().Wait();
                }
                else
                {
                    _lichessClient.SendInputAsync(command.Text).Wait();
                }
            }
        }
    }
}
