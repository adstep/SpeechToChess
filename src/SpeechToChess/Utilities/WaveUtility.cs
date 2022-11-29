using NAudio.Wave;

namespace SpeechToChess.Utilities
{
    public class WaveUtility
    {
        public static void Trim(Stream input, Stream output, TimeSpan start, TimeSpan end)
        {
            WaveFileReader reader = new WaveFileReader(input);
            WaveFileWriter writer = new WaveFileWriter(output, reader.WaveFormat);

            float bytesPerMs = reader.WaveFormat.AverageBytesPerSecond / 1000f;

            int startPos = (int)Math.Round(start.TotalMilliseconds * bytesPerMs);
            startPos = startPos - startPos % reader.WaveFormat.BlockAlign;

            int endPos = (int)Math.Round(end.TotalMilliseconds * bytesPerMs);
            endPos = endPos - endPos % reader.WaveFormat.BlockAlign;

            Trim(reader, writer, startPos, endPos);
            writer.Flush();
            input.Position = 0;
            output.Position = 0;
        }

        private static void Trim(WaveFileReader reader, WaveFileWriter writer, int startPos, int endPos)
        {
            reader.Position = startPos;
            byte[] buffer = new byte[reader.BlockAlign * 1024];
            while (reader.Position < endPos)
            {
                int bytesRequired = (int)(endPos - reader.Position);
                if (bytesRequired > 0)
                {
                    int bytesToRead = Math.Min(bytesRequired, buffer.Length);
                    int bytesRead = reader.Read(buffer, 0, bytesToRead);
                    if (bytesRead > 0)
                    {
                        writer.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }
    }
}
