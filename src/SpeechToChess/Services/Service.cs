using Microsoft.Extensions.Options;
using SpeechToChess.Clients;
using SpeechToChess.Models.Cognitive;
using SpeechToChess.Models.Commands;
using SpeechToChess.Models.Configuration;
using SpeechToChess.Models.Speech;
using SpeechToChess.Models.Transformers;
using SpeechToChess.Utilities;

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

        public Task RunArchive()
        {
            ICommandTransformer commandTransformer = new TranscriptionTransformer();

            const string archivePath = @"C:\Users\adstep\Downloads\2022-11-25-10-52-53_2022-11-28-03-00-24.zip";
            const string trainPath = "train.zip";
            LogArchive logArchive = LogArchive.Load(archivePath);
            TrainLogArchive trainLogArchive = TrainLogArchive.CreateOrLoad(trainPath);

            foreach (string entryName in logArchive.Entries)
            {
                LogEntry logEntry = logArchive.Read(entryName);
                int index = 0;

                foreach (RecognitionResult recognitionResult in logEntry.RecognitionResults.RecognizedPhrases)
                {
                    try
                    {
                        string requestId = logEntry.RecognitionResults.RequestId;
                        string fileName = $"{requestId}_{index++}.wav";

                        if (trainLogArchive.Contains(fileName))
                        {
                            Console.WriteLine($"Skipping {fileName}");
                            continue;
                        }

                        TimeSpan start = (recognitionResult.Offset < TimeSpan.FromSeconds(0.25)) ?
                            recognitionResult.Offset :
                            recognitionResult.Offset + TimeSpan.FromSeconds(-0.25);
                        TimeSpan end = recognitionResult.Offset + recognitionResult.Duration + TimeSpan.FromSeconds(0.25);

                        using MemoryStream stream = new MemoryStream();
                        WaveUtility.Trim(logEntry.AudioStream, stream, start, end);

                        string transcription = recognitionResult.NBest.First().Display;

                        Console.WriteLine($"Read: {transcription}");
                        transcription = commandTransformer.Apply(transcription);
                        Console.WriteLine($"Transform: {transcription}");

                        Audio.Play(stream);

                        string? command = Console.ReadLine();

                        if (string.IsNullOrEmpty(command))
                        {
                            Console.WriteLine($"Adding '{fileName}' as '{transcription}'");
                            trainLogArchive.Add(stream, fileName, transcription);
                        }
                        else if (command == "s")
                        {
                            // Skip
                            Console.WriteLine("Skipping");
                            trainLogArchive.Exclude(fileName);
                            continue;
                        }
                        else
                        {
                            transcription = command;
                            Console.WriteLine($"Adding '{fileName}' as '{transcription}'");
                            trainLogArchive.Add(stream, fileName, transcription);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Skipping due to error");
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

            trainLogArchive.Save(trainPath);


            return Task.CompletedTask;
        }

        public async Task Run()
        {
            await RunArchive();

            return;

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

                //if (command is CloseCommand)
                //{
                //    _isActive = false;
                //    _speechRecognizer.Recognized -= OnSpeechRecognized;
                //}
                //else if (command is ClearCommand)
                //{
                //    _lichessClient.ClearInputAsync().Wait();
                //}
                //else if (command is PuzzleCommand)
                //{
                //    _lichessClient.NavigateToPuzzle().Wait();
                //}
                //else if (command is HomeCommand)
                //{
                //    _lichessClient.NavigateToHome().Wait();
                //}
                //else
                //{
                //    _lichessClient.SendInputAsync(command.Text).Wait();
                //}
            }
        }
    }
}
