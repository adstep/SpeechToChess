using Microsoft.Playwright;
using SpeechToChess.Utilities;
using System.Diagnostics;

namespace SpeechToChess.Clients
{
    public class PlaywrightLichessClient : ILichessClient
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;

        public Task Init()
        {
            Microsoft.Playwright.Program.Main(new string[] { "install" });
            return Task.CompletedTask;
        }

        public async Task LaunchAsync()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = false,
            });
            
            _page = await _browser.NewPageAsync(new BrowserNewPageOptions() { });
            DisplayUtility.GetDisplaySize(out int width, out int height);
            await _page.SetViewportSizeAsync(width, height - 120);

            Process process = Process.GetProcesses().First(p => p.MainWindowTitle.Contains("Chromium"));
            WindowUtility.Maximize(process.MainWindowTitle);

            await NavigateToHome();
        }

        public async Task NavigateToHome()
        {
            await _page.GotoAsync("https://lichess.org/");
        }

        public async Task NavigateToPuzzle()
        {
            await _page.GotoAsync("https://lichess.org/training");
        }

        public async Task LoginAsync(string userName, string password)
        {
            await SignInAsync();

            await SetUserNameAsync(userName);
            await SetPasswordAsync(password);

            await SubmitLoginAsync();
        }

        public async Task SendInputAsync(string command)
        {
            await ClearInputAsync();
            await _page.Locator("input.ready").TypeAsync(command, new LocatorTypeOptions() { Timeout = 1000f });
        }

        public async Task ClearInputAsync()
        {
            await _page.Locator("input.ready").ClearAsync(new LocatorClearOptions() { Timeout = 1000f });
        }

        private async Task SignInAsync()
        {
            await _page.Locator("a.signin.button.button-empty").ClickAsync();
        }

        private async Task SetUserNameAsync(string userName)
        {
            await _page.Locator("#form3-username").TypeAsync(userName);
        }

        private async Task SetPasswordAsync(string password)
        {
            await _page.Locator("#form3-password").TypeAsync(password);
        }

        private async Task SubmitLoginAsync()
        {
            await _page.Locator("div.one-factor button.submit.button").ClickAsync();
        }
    }
}
