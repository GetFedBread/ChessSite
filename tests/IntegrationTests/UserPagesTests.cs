namespace IntegrationTests;

public class UserPagesTests(ChessWebApplicationFactory factory) : IClassFixture<ChessWebApplicationFactory>
{
    private readonly ChessWebApplicationFactory _factory = factory;

    [Fact]
    public async Task UsersPageContainsUsers()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/users");
        
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        // Check for title
        Assert.Contains("Users", content);

        // Check for seeded usernames
        Assert.Contains("John Doe", content);
        Assert.Contains("Jane Doe", content);
        Assert.Contains("Michelangelo", content);
    }

    [Fact]
    public async Task LoggedOutUserPageContainsCorrectInfo()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/users/Michelangelo");

        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Michelangelo", content);

        // Michelangelo has no games
        Assert.Contains("This user has no games", content);

        // We are not logged in, so there shouldn't be a friend request button
        Assert.DoesNotContain("Send friend request", content);
    }
}
