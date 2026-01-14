namespace E2ETests;

using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
//using NuGet.Protocol;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    private Process _serverProcess;
    private string _url = "http://localhost:5163/";
    public override BrowserNewContextOptions ContextOptions() => new() { IgnoreHTTPSErrors = true };

    [OneTimeSetUp]
    public async Task Init()
    {
        string projectPath = "../../../../../src/Web/ChessSite.csproj";
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --project \"{projectPath}\" --launch-profile testing",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        _serverProcess = Process.Start(startInfo)!;

        _serverProcess.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
        _serverProcess.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);
        

        // Wait for server to start
        string? line;
        while ((line = await _serverProcess.StandardOutput.ReadLineAsync()) != null)
        {
            Console.WriteLine(line);

            if (line.Contains("Now listening on", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Server ready");
                break;
            }
        }

        
    }

    [Test]
    public async Task ChessboardTilesCorrectWhite()
    {
        await Page.GotoAsync(_url);
        var tiles = Page.Locator("#Chessboard tbody tr th[id]");
        var tileCount = await tiles.CountAsync();

        int expectedRow = 8;
        char expectedColumn = 'a';
        for(int i = 0; i < tileCount; i++)
        {
            var id = await tiles.Nth(i).GetAttributeAsync("id");
            var color = await tiles.Nth(i).GetAttributeAsync("class");

            string expectedId = ""+expectedColumn+expectedRow;
            
            bool isWhiteSquare = (expectedRow + expectedColumn - 'a') % 2 == 0;
            string expectedColor = isWhiteSquare ? "w" : "b";

            Assert.That(id, Is.EqualTo(expectedId));
            Assert.That(color, Is.EqualTo(expectedColor));

            expectedColumn++;
            if(expectedColumn > 'h')
            {
                expectedRow--;
                expectedColumn = 'a';
            }
        }
    }

    [Test]
    public async Task ChessboardTilesCorrectBlack()
    {
        await Page.GotoAsync(_url+"?fromWhite=false");
        var tiles = Page.Locator("#Chessboard tbody tr th[id]");
        var tileCount = await tiles.CountAsync();

        int expectedRow = 1;
        char expectedColumn = 'h';
        for(int i = 0; i < tileCount; i++)
        {
            var id = await tiles.Nth(i).GetAttributeAsync("id");
            var color = await tiles.Nth(i).GetAttributeAsync("class");

            string expectedId = ""+expectedColumn+expectedRow;

            bool isWhiteSquare = (expectedRow + expectedColumn - 'a') % 2 == 0;
            string expectedColor = isWhiteSquare ? "w" : "b";

            Assert.That(id, Is.EqualTo(expectedId));
            Assert.That(color, Is.EqualTo(expectedColor));

            expectedColumn--;
            if(expectedColumn < 'a')
            {
                expectedRow++;
                expectedColumn = 'h';
            }
        }
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _serverProcess.Kill(entireProcessTree: true);
        _serverProcess.Dispose();
    }
}
