using System.IO.Compression;
using System.Text.Json;

namespace SpeechToChess.Models.Cognitive
{
    public class LogArchive : IDisposable
    {
        private bool _disposed;
        private ZipArchive _zipArchive;

        public List<string> Entries { get; private set; }

        protected LogArchive(ZipArchive zipArchive)
        {
            _zipArchive = zipArchive;

            Entries = _zipArchive.Entries
                .Select(e => e.Name)
                .Where(n => !n.Contains("v2"))
                .Distinct()
                .Select(n => Path.GetFileNameWithoutExtension(n))
                .ToList();
        }

        public LogEntry Read(string entry)
        {
            string audioEntryName = $"{entry}.wav";
            string metadataEntryName = $"{entry}.v2.json";

            ZipArchiveEntry? audioEntry = _zipArchive.GetEntry(audioEntryName);
            ZipArchiveEntry? metadataEntry = _zipArchive.GetEntry(metadataEntryName);

            using Stream audioEntryStream = audioEntry.Open();
            MemoryStream audioStream = new MemoryStream();
            audioEntryStream.CopyTo(audioStream);
            audioStream.Position = 0;

            using Stream metadataStream = metadataEntry.Open();
            //string json = new StreamReader(metadataStream).ReadToEnd();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            //RecognitionResults test = JsonSerializer.Deserialize<RecognitionResults>(json, options);

            RecognitionResults? recognitionResults = JsonSerializer.Deserialize<RecognitionResults>(metadataStream, options);

            return new LogEntry()
            {
                AudioStream = audioStream,
                RecognitionResults = recognitionResults
            };
        }

        public static LogArchive Load(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"No log archive exists at '{filePath}'");

            ZipArchive zipArchive = ZipFile.OpenRead(filePath);

            return new LogArchive(zipArchive);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _zipArchive.Dispose();
            }
            _disposed = true;
        }
    }
}
