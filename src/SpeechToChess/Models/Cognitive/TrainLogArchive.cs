using System.IO.Compression;

namespace SpeechToChess.Models.Cognitive
{
    public class TrainLogArchive
    {
        private ZipArchive _zipArchive;

        public HashSet<string> FileNames { get; private set; }
        public HashSet<string> Excludes { get; private set; }

        protected TrainLogArchive(ZipArchive zipArchive)
        {
            _zipArchive = zipArchive;

            ZipArchiveEntry? transcriptionEntry = _zipArchive.GetEntry("transcription.tsv");

            if (transcriptionEntry == null)
            {
                transcriptionEntry = _zipArchive.CreateEntry("transcription.tsv");
            }

            using StreamReader streamReader = new StreamReader(transcriptionEntry.Open());

            FileNames = new HashSet<string>();

            while (!streamReader.EndOfStream)
            {
                string? line = streamReader.ReadLine();

                if (line == null)
                    continue;

                FileNames.Add(line.Split('\t')[0]);
            }

            ZipArchiveEntry? exclusionEntry = _zipArchive.GetEntry("exclusion.tsv");

            if (exclusionEntry == null)
            {
                exclusionEntry = _zipArchive.CreateEntry("exclusion.tsv");
            }

            using StreamReader exclusionStreamReader = new StreamReader(exclusionEntry.Open());

            Excludes = new HashSet<string>();

            while (!exclusionStreamReader.EndOfStream)
            {
                string? line = exclusionStreamReader.ReadLine();

                if (line == null)
                    continue;

                Excludes.Add(line.Split('\t')[0]);
            }
        }

        public void Add(Stream audioStream, string fileName, string transcription)
        {
            string transcriptionName = "transcription.tsv";

            ZipArchiveEntry audioEntry = _zipArchive.CreateEntry(fileName);
            using Stream audioEntryStream = audioEntry.Open();
            audioStream.Position = 0;
            audioStream.CopyTo(audioEntryStream);

            ZipArchiveEntry? transcriptionEntry = _zipArchive.GetEntry(transcriptionName);
            using Stream transcriptionEntryStream = transcriptionEntry.Open();
            transcriptionEntryStream.Seek(0, SeekOrigin.End);
            using StreamWriter streamWriter = new StreamWriter(transcriptionEntryStream);
            streamWriter.WriteLine($"{fileName}\t{transcription}");
        }

        public void Exclude(string fileName)
        {
            string exclusionName = "exclusion.tsv";

            ZipArchiveEntry? transcriptionEntry = _zipArchive.GetEntry(exclusionName);
            using Stream transcriptionEntryStream = transcriptionEntry.Open();
            transcriptionEntryStream.Seek(0, SeekOrigin.End);
            using StreamWriter streamWriter = new StreamWriter(transcriptionEntryStream);
            streamWriter.WriteLine($"{fileName}");
        }

        public bool Contains(string fileName)
        {
            return FileNames.Contains(fileName) || Excludes.Contains(fileName);
        }

        public static TrainLogArchive CreateOrLoad(string filePath)
        {
            if (!File.Exists(filePath))
            {
                ZipFile.Open(filePath, ZipArchiveMode.Create).Dispose();
            }

            ZipArchive zipArchive = ZipFile.Open(filePath, ZipArchiveMode.Update);
            return new TrainLogArchive(zipArchive);
        }

        public void Save(string filePath)
        {
            _zipArchive.Dispose();
        }
    }
}
