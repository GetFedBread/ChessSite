namespace Infrastructure;

using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class UserRepository : IUserRepository
{
    private readonly ChessDbContext _dbContext;
    public UserRepository(ChessDbContext context)
    {
        _dbContext = context;

    }

    public async Task<UserDTO?> GetUserByName(string username)
    {
        var query = from user in _dbContext.Users
                .Include(a => a.GamesAsBlack)
                .Include(a => a.GamesAsWhite)
                    where user.UserName == username
                    select user;

        var result = await query.FirstOrDefaultAsync();
        if (result == null) return null;

        return new UserDTO
        {
            Username = result.UserName,
            Games = result.GamesAsWhite.Concat(result.GamesAsBlack)
            .Select(g => new GameDTO
            {
                StartingPosition = g.StartingPosition,
                Moves = g.Moves,
                GameId = g.GameId,
                Black = new UserDTO
                {
                    Username = g.Black == null ? "Deleted" : g.Black.UserName
                },
                White = new UserDTO
                {
                    Username = g.White == null ? "Deleted" : g.White.UserName
                }

            }).ToList()
        };
    }

    public async Task<List<UserDTO>> GetUsers(int count, int offset, string? search)
    {
        var query = from user in _dbContext.Users
            where user.UserName.Contains(search ?? "")
            orderby user.UserName descending
                select new UserDTO
                {
                    Username = user.UserName

                };

        return await query.Skip(offset).Take(count).ToListAsync();
    }
}