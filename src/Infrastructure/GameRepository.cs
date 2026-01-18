
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Infrastructure;

public class GameRepository(ChessDbContext context) : IGameRepository
{
    private readonly ChessDbContext _dbContext = context;
    public async Task<GameDTO?> GetGameById(int gameId)
    {
        var query = from game in _dbContext.Games
            .Include(g => g.White)
            .Include(g => g.Black)

        where game.GameId == gameId
        select new GameDTO
        {
            GameId = game.GameId,
            StartDate = game.StartDate,
            Active = game.Active,
            StartingPosition = game.StartingPosition,
            Moves = game.Moves,
            White = new UserDTO
            {
                Username = game.White == null ? "Deleted" : game.White.UserName
            },
            Black = new UserDTO
            {
                Username = game.Black == null ? "Deleted" : game.Black.UserName
            }
        };

        return await query.FirstOrDefaultAsync();
    }
}