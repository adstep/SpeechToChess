using NAudio.Wave;

namespace SpeechToChess.Utilities
{
    public class Audio
    {
        public static void Play(Stream audioStream)
        {
            using WaveFileReader waveFileReader = new WaveFileReader(audioStream);
            using WaveOutEvent outputDevice = new WaveOutEvent();
            outputDevice.Init(waveFileReader);

            outputDevice.Play();

            while (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
