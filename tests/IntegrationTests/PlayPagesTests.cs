namespace IntegrationTests;

public class PlayPagesTests(ChessWebApplicationFactory factory) : IClassFixture<ChessWebApplicationFactory>
{
    private readonly ChessWebApplicationFactory _factory = factory;

    [Fact]
    public async Task PlaySelfHasChessboard()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/play/self");

        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chessboard", content);

        // Check for all tile id's
        for(int row = 1; row <= 8; row++)
        {
            for(char column = 'a'; column <= 'h'; column++)
            {
                Assert.Contains(""+column+row, content);
            }
        }
    }
}
