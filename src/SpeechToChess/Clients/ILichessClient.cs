namespace SpeechToChess.Clients
{
    public interface ILichessClient
    {
        Task Init();
        Task LaunchAsync();
        Task LoginAsync(string userName, string password);
        Task NavigateToPuzzle();
        Task NavigateToHome();
        Task SendInputAsync(string command);
        Task ClearInputAsync();
    }
}
