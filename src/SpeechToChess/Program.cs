using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SpeechToChess.Clients;
using SpeechToChess.Models;
using SpeechToChess.Models.Commands;
using SpeechToChess.Models.Configuration;
using SpeechToChess.Models.Speech;
using SpeechToChess.Models.Transformers;
using SpeechToChess.Services;
using CommandOptions = SpeechToChess.Models.CommandOptions;

namespace SpeechToChess
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<CommandOptions>(args)
                .WithParsedAsync(async o =>
                {
                    try
                    {
                        var host = CreateHostBuilder(o)
                            .UseConsoleLifetime()
                            .Build();

                        await host.Services.GetService<IService>()!.Run();
                    }
                    catch (OptionsValidationException ex)
                    {
                        Console.WriteLine(ex);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
        }

        private static IHostBuilder CreateHostBuilder(CommandOptions options)
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile($"appsettings.local.json");
                    builder.AddJsonFile($"appsettings.secret.json", true);
                })
                .ConfigureServices((context, services) =>
                {
                    if (options.Engine == RecognitionEngineType.Cognitive) {
                        services.AddOptions<CognitiveOptions>()
                            .Bind(context.Configuration.GetSection(CognitiveOptions.Cognitive))
                            .ValidateDataAnnotations();
                    }

                    OptionsBuilder<LichessOptions> builder = services.AddOptions<LichessOptions>()
                        .Bind(context.Configuration.GetSection(LichessOptions.Lichess))
                        .ValidateDataAnnotations();

                    services.AddSingleton<IService, Service>();

                    services.AddSingleton<ILichessClient, PlaywrightLichessClient>();
                    services.AddSingleton<ICommandResolver, CommandResolver>();
                    services.AddSingleton<ICommandTransformer, CommandTransformer>();

                    if (options.Engine == RecognitionEngineType.Cognitive)
                    {
                        services.AddSingleton<ISpeechRecognizer, CognitiveSpeechRecognizer>();
                    }
                    else if (options.Engine == RecognitionEngineType.Local)
                    {
                        services.AddSingleton<ISpeechRecognizer, LocalSpeechRecognizer>();
                    }
                });

            return hostBuilder;
        }
    }
}