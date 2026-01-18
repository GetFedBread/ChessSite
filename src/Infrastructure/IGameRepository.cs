namespace Infrastructure;

public interface IGameRepository
{
    public Task<GameDTO?> GetGameById(int gameId);
}