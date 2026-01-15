namespace IntegrationTests;

public class MiscPagesTests(ChessWebApplicationFactory factory) : IClassFixture<ChessWebApplicationFactory>
{
    private readonly ChessWebApplicationFactory _factory = factory;

    [Fact]
    public async Task RootReturnsCorrectHeaders()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/");

        response.EnsureSuccessStatusCode();

        Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }
}
